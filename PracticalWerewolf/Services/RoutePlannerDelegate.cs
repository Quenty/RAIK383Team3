using PracticalWerewolf.Controllers;
using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Device.Location;
using GoogleMapsApi.Entities.Directions.Response;
using PracticalWerewolf.Models.Trucks;
using System.Linq;

namespace PracticalWerewolf.Services
{
    public class RoutePlannerDelegate
    {

        public ContractorInfo Contractor { get; set; }
        public Order Order { get; set; }
        public List<RouteStop> Route { get; set; }
        public RouteStop PickUp { get; }
        public RouteStop DropOff { get; }
        public int DistanceChanged { get; set; }

        private Dictionary<CivicAddressDb, DirectionsResponse> _pickUpToStop = new Dictionary<CivicAddressDb, DirectionsResponse>();
        private Dictionary<CivicAddressDb, DirectionsResponse> _dropOffToStop = new Dictionary<CivicAddressDb, DirectionsResponse>();
        private List<TruckCapacityUnit> _capacityUnits = new List<TruckCapacityUnit>(); //these will be the capacities immediately after the stop corresponding to the index



        public RoutePlannerDelegate(ContractorInfo contractor, Order order, List<RouteStop> route)
        {
            Contractor = contractor;
            Order = order;
            Route = route;

            PickUp = new RouteStop
            {
                RouteStopGuid = Guid.NewGuid(),
                Order = Order,
                Type = StopType.PickUp
            };

            DropOff = new RouteStop
            {
                RouteStopGuid = Guid.NewGuid(),
                Order = Order,
                Type = StopType.DropOff
            };
        }

        public void CalculateOptimalRoute()
        {
            if (!Route.Any())
            {
                DeliverFromHomeAddress();
                return;
            }
            
            GetDistances();
            GetTruckCapacityUnits();

            var sublists = GetSubRoutesWithNoCapacityConflicts();
            var results = new List<SubRouteCalculationResult>();

            foreach(var sublist in sublists)
            {
                var result = GetBestPickUpAndDropOffPlacementInSublist(sublist.Item1, sublist.Item2);
                results.Add(result);
            }
            //this shows us what happens if we pick up and drop off order at the end of the route
            results.Add(AddPickUpAndDropOffToEndOfRoute());

            var best = results.First();
            foreach(var result in results)
            {
                if(best.DistanceChanged > result.DistanceChanged)
                {
                    best = result;
                }
            }

            DistanceChanged = best.DistanceChanged;

            Route.Insert(best.DropOffIndex, DropOff);
            var prev = Route[best.DropOffIndex - 1];
            prev.DistanceToNextStop = GetDistanceFromDropOff(prev);
            prev.EstimatedTimeToNextStop = GetTimeFromDropOff(prev);

            Route.Insert(best.PickUpIndex, PickUp);
            prev = Route[best.PickUpIndex - 1];
            prev.DistanceToNextStop = GetDistanceFromPickUp(prev);
            prev.EstimatedTimeToNextStop = GetTimeFromPickUp(prev);

            OrderRouteStops();
        }




        private void GetDistances()
        {
            var pickUpLocation = Order.RequestInfo.PickUpAddress;
            var dropOffLocation = Order.RequestInfo.DropOffAddress;

            foreach(var stop in Route)
            {
                var location = (stop.Type == StopType.PickUp) ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;

                if (!_pickUpToStop.ContainsKey(location))
                {
                    var response = LocationHelper.GetRouteBetweenLocations(pickUpLocation, location);
                    _pickUpToStop.Add(location, response);
                }
            }

            _pickUpToStop.Add(dropOffLocation, LocationHelper.GetRouteBetweenLocations(pickUpLocation, dropOffLocation));


            foreach (var stop in Route)
            {
                var location = (stop.Type == StopType.PickUp) ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;

                if (!_dropOffToStop.ContainsKey(location))
                {
                    var response = LocationHelper.GetRouteBetweenLocations(dropOffLocation, location);
                    _dropOffToStop.Add(location, response);
                }
            }
        }

        private void GetTruckCapacityUnits()
        {
            TruckCapacityUnit current = Contractor.Truck.UsedCapacity;

            foreach(var stop in Route)
            {
                TruckCapacityUnit newCapacity;
                if(stop.Type == StopType.PickUp)
                {
                    newCapacity = AddCapacities(current, stop.Order.RequestInfo.Size);
                }
                else
                {
                    newCapacity = RemoveCapacities(current, stop.Order.RequestInfo.Size);
                }

                _capacityUnits.Add(newCapacity);

                current = newCapacity;
            }
        }


        private IEnumerable<Tuple<int, int>> GetSubRoutesWithNoCapacityConflicts()
        {
            List<Tuple<int, int>> sublists = new List<Tuple<int, int>>();
            int count = Route.Count;
            TruckCapacityUnit pickUpSize = Order.RequestInfo.Size;
            int lastFirstSublistStop = 0;

            for(int i = 0; i < count; i++)
            {
                TruckCapacityUnit capacityWithPickUp = AddCapacities(_capacityUnits[i], pickUpSize);

                if (!IsWithinMaxCapacity(capacityWithPickUp, Contractor.Truck.MaxCapacity))
                {
                    var number = i - lastFirstSublistStop + 1;  //note: we include the last stop even though we can't drop it off after. we include it so that we can calculate the additional distance
                    if (number > 1)
                    {
                        sublists.Add(new Tuple<int, int>(lastFirstSublistStop, i));
                    }
                    lastFirstSublistStop = i + 1;
                }
            }

            var lastNumber = count - lastFirstSublistStop;
            if (lastNumber > 1)
            {
                var sublist = Route.GetRange(lastFirstSublistStop, lastNumber);
                sublists.Add(new Tuple<int, int>(lastFirstSublistStop, count));
            }

            return sublists;
        }

        private SubRouteCalculationResult GetBestPickUpAndDropOffPlacementInSublist(int start, int end)
        {
            var count = end - start + 1;
            var best = new SubRouteCalculationResult();

            //i and j are the indexes we would put them 
            for(int i = 1; i < count; i++)
            {
                for(int j = i; j < count; j++)
                {
                    //calculate new distance
                    int totalDistance = 0;
                    int previousDistance = 0;

                    var prev = Route[i - 1];
                    var distanceToPickUp = GetDistanceFromPickUp(prev);
                    totalDistance += distanceToPickUp;

                    previousDistance += prev.DistanceToNextStop;

                    var next = Route[j + 1];
                    var distanceFromDropOff = GetDistanceFromDropOff(next);
                    totalDistance += distanceFromDropOff;

                    if (i == j)
                    {
                        //if pick up and drop off happen between the same two stops
                        var pickUpToDropOffDistance = GetDistanceFromPickUp(DropOff);
                        totalDistance += pickUpToDropOffDistance;
                    }
                    else
                    {
                        var afterPickUp = Route[i + 1];
                        var afterPickUpDistance = GetDistanceFromPickUp(afterPickUp);
                        totalDistance += afterPickUpDistance;

                        var beforeDropOff = Route[j - 1];
                        var beforeDropOffDistance = GetDistanceFromDropOff(beforeDropOff);
                        totalDistance += beforeDropOffDistance;

                        previousDistance += beforeDropOff.DistanceToNextStop;
                    }

                    //check if qualifies
                    int newChange = totalDistance - previousDistance;
                    if(newChange < best.DistanceChanged)
                    {
                        best.DistanceChanged = newChange;
                        best.PickUpIndex = i;
                        best.DropOffIndex = j;
                    }
                }
            }

            return best;
        }

        private SubRouteCalculationResult AddPickUpAndDropOffToEndOfRoute()
        {
            var end = Route.Last();
            var endToPickUpDistance = GetDistanceFromPickUp(end);
            var pickUpToDropOffDistance = GetDistanceFromPickUp(DropOff);

            return new SubRouteCalculationResult()
            {
                DistanceChanged = endToPickUpDistance + pickUpToDropOffDistance,
                DropOffIndex = Route.Count,
                PickUpIndex = Route.Count
            };
        }

        private void DeliverFromHomeAddress()
        {
            var home = Contractor.Truck.Location;
            var homeAddress = string.Format($"{home.Latitude}, {home.Longitude}");

            var result = LocationHelper.GetRouteBetweenLocations(homeAddress, Order.RequestInfo.PickUpAddress.RawInputAddress);
            PickUp.DistanceToNextStop = GetDistanceFromPickUp(DropOff);
            PickUp.EstimatedTimeToNextStop = GetTimeFromPickUp(DropOff);

            DistanceChanged = result.Routes.First().Legs.First().Distance.Value + PickUp.DistanceToNextStop;
            Route.Add(PickUp);
            Route.Add(DropOff);

            OrderRouteStops();
        }

        private int GetDistanceFromPickUp(RouteStop stop)
        {
            var address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;
            return _pickUpToStop[address].Routes.First().Legs.First().Distance.Value;
        }

        private int GetDistanceFromDropOff(RouteStop stop)
        {
            var address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;
            return _dropOffToStop[address].Routes.First().Legs.First().Distance.Value;
        }

        private TimeSpan GetTimeFromDropOff(RouteStop stop)
        {
            var address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;
            return _dropOffToStop[address].Routes.First().Legs.First().Duration.Value;
        }

        private TimeSpan GetTimeFromPickUp(RouteStop stop)
        {
            var address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;
            return _pickUpToStop[address].Routes.First().Legs.First().Duration.Value;
        }

        private TruckCapacityUnit AddCapacities(TruckCapacityUnit capacity1, TruckCapacityUnit capacity2)
        {
            return new TruckCapacityUnit
            {
                Mass = capacity1.Mass + capacity2.Mass,
                Volume = capacity1.Volume + capacity2.Volume
            };
        }

        private TruckCapacityUnit RemoveCapacities(TruckCapacityUnit capacity1, TruckCapacityUnit capacity2)
        {
            return new TruckCapacityUnit
            {
                Mass = capacity1.Mass - capacity2.Mass,
                Volume = capacity1.Volume - capacity2.Volume
            };
        }

        private bool IsWithinMaxCapacity(TruckCapacityUnit capacity, TruckCapacityUnit max)
        {
            return (capacity.Mass <= max.Mass) && (capacity.Volume <= max.Volume);
        }

        private void OrderRouteStops()
        {
            for(int i = 0; i < Route.Count; i++)
            {
                Route[i].StopOrder = i;
            }
        }


        private class SubRouteCalculationResult
        {
            public int PickUpIndex;
            public int DropOffIndex;
            public int DistanceChanged = int.MaxValue;
        }

    }
}