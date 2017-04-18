using PracticalWerewolf.Models.Orders;
using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using System.Device.Location;
using GoogleMapsApi.Entities.Directions.Response;
using PracticalWerewolf.Models.Trucks;
using System.Linq;
using PracticalWerewolf.Helpers;
using log4net;

namespace PracticalWerewolf.Services
{
    public class ContractorRoutePlanner
    {
        private static ILog logger = LogManager.GetLogger(typeof(ContractorRoutePlanner)); 
        public bool WillWork = true;
        public ContractorInfo Contractor { get; }
        public Order Order { get; }
        public List<RouteStop> Route { get; }
        public RouteStop PickUp { get; }
        public RouteStop DropOff { get; }
        public int DistanceChanged { get; set; }

        private Dictionary<CivicAddressDb, DirectionsResponse> _pickUpToStop = new Dictionary<CivicAddressDb, DirectionsResponse>();
        private Dictionary<CivicAddressDb, DirectionsResponse> _dropOffToStop = new Dictionary<CivicAddressDb, DirectionsResponse>();
        private List<TruckCapacityUnit> _capacityUnits = new List<TruckCapacityUnit>(); //these will be the capacities immediately after the stop corresponding to the index



        public ContractorRoutePlanner(ContractorInfo contractor, Order order, List<RouteStop> route)
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
            if(!IsWithinMaxCapacity(Order.RequestInfo.Size, Contractor.Truck.MaxCapacity))
            {
                WillWork = false;
                return;
            }

            GetDistances();

            if (!Route.Any())
            {
                DeliverFromHomeAddress();
                return;
            }
            
            GetTruckCapacityUnits();

            IEnumerable<Tuple<int, int>> sublists = GetSubRoutesWithNoCapacityConflicts();
            List<SubRouteCalculationResult> results = new List<SubRouteCalculationResult>();

            foreach(var sublist in sublists)
            {
                SubRouteCalculationResult result = GetBestPickUpAndDropOffPlacementInSublist(sublist.Item1, sublist.Item2);
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

            //insert pickup and dropoff into route

            Route.Insert(best.DropOffIndex, DropOff);

            //set previous numbers
            Route.Insert(best.PickUpIndex, PickUp);
            RouteStop prev = Route[best.PickUpIndex - 1];
            prev.DistanceToNextStop = GetDistanceFromPickUp(prev);
            prev.EstimatedTimeToNextStop = GetTimeFromPickUp(prev);

            //set PickUp numbers
            var next = Route[best.PickUpIndex + 1];
            PickUp.DistanceToNextStop = GetDistanceFromPickUp(next);
            PickUp.EstimatedTimeToNextStop = GetTimeFromPickUp(next);

            //update distance to DropOff here in case we have to insert PickUp first
            //not DropOffIndex-1 because we inserted PickUp before it
            prev = Route[best.DropOffIndex];
            prev.DistanceToNextStop = GetDistanceFromDropOff(prev);
            prev.EstimatedTimeToNextStop = GetTimeFromDropOff(prev);

            //update DropOff index if it's not the last one
            if (!Route.Last().Equals(DropOff))
            {
                int nextIndex = Route.IndexOf(DropOff) + 1;
                next = Route[nextIndex];
                DropOff.DistanceToNextStop = GetDistanceFromDropOff(next);
                DropOff.EstimatedTimeToNextStop = GetTimeFromDropOff(next);
            }

            OrderRouteStops();
        }




        private void GetDistances()
        {
            var pickUpLocation = Order.RequestInfo.PickUpAddress;
            var dropOffLocation = Order.RequestInfo.DropOffAddress;

            //PickUp location to all stops
            foreach(var stop in Route)
            {
                var location = (stop.Type == StopType.PickUp) ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;

                if (!_pickUpToStop.ContainsKey(location))
                {
                    var response = LocationHelper.GetRouteBetweenLocations(pickUpLocation, location);
                    if(response.Status == DirectionsStatusCodes.OK)
                    {
                        _pickUpToStop.Add(location, response);
                    }
                    else
                    {
                        logger.Warn($"Google Maps Api failed to load address {location}. Error is {response.Status}");
                    }
                }
            }

            //PickUp to DropOff
            if (!_pickUpToStop.ContainsKey(dropOffLocation))
            {
                var response = LocationHelper.GetRouteBetweenLocations(pickUpLocation, dropOffLocation);
                if (response.Status == DirectionsStatusCodes.OK)
                {
                    _pickUpToStop.Add(dropOffLocation, response);
                }
                else
                {
                    logger.Warn($"Google Maps Api failed to load address {dropOffLocation}. Error is {response.Status}");
                }
            }
            //DropOff to PickUp
            if (!_dropOffToStop.ContainsKey(pickUpLocation))
            {
                var response = LocationHelper.GetRouteBetweenLocations(dropOffLocation, pickUpLocation);
                if (response.Status == DirectionsStatusCodes.OK)
                {
                    _dropOffToStop.Add(pickUpLocation, response);
                }
                else
                {
                    logger.Warn($"Google Maps Api failed to load address {dropOffLocation}. Error is {response.Status}");
                }
            }


            //DropOff location to all stops
            foreach (var stop in Route)
            {
                var location = (stop.Type == StopType.PickUp) ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;

                if (!_dropOffToStop.ContainsKey(location))
                {
                    var response = LocationHelper.GetRouteBetweenLocations(dropOffLocation, location);
                    if (response.Status == DirectionsStatusCodes.OK)
                    {
                        _dropOffToStop.Add(location, response);
                    }
                    else
                    {
                        logger.Warn($"Google Maps Api failed to load address {dropOffLocation}. Error is {response.Status}");
                    }
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
            var best = new SubRouteCalculationResult();

            //i and j are the indexes we would put them 
            for(int i = start + 1; i <= end; i++)
            {
                for(int j = i; j <= end; j++)
                {
                    //calculate new distance
                    int totalDistance = 0;
                    int previousDistance = 0;

                    var prev = Route[i - 1];
                    var distanceToPickUp = GetDistanceFromPickUp(prev);
                    totalDistance += distanceToPickUp;

                    previousDistance += prev.DistanceToNextStop;

                    if (j < Route.Count)
                    {
                        var next = Route[j];
                        var distanceFromDropOff = GetDistanceFromDropOff(next);
                        totalDistance += distanceFromDropOff;
                    }

                    if (i == j)
                    {
                        //if pick up and drop off happen between the same two stops
                        var pickUpToDropOffDistance = GetDistanceFromPickUp(DropOff);
                        totalDistance += pickUpToDropOffDistance;
                    }
                    else
                    {
                        var afterPickUp = Route[i];
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
            var result = LocationHelper.GetRouteBetweenLocations(Contractor.HomeAddress, Order.RequestInfo.PickUpAddress);
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

            if (address.Equals(Order.RequestInfo.PickUpAddress))
            {
                return 0;
            }
            else
            {
                if (_pickUpToStop.ContainsKey(address))
                {
                    return _pickUpToStop[address].Routes.First().Legs.First().Distance.Value;
                }
                else
                {
                    logger.Warn($"Failed to retrieve address {address.ToString()}");
                    return int.MaxValue;
                }
            }
        }

        private int GetDistanceFromDropOff(RouteStop stop)
        {
            var address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;

            if (address.Equals(Order.RequestInfo.DropOffAddress))
            {
                return 0;
            }
            else
            {
                if (_dropOffToStop.ContainsKey(address))
                {
                    return _dropOffToStop[address].Routes.First().Legs.First().Distance.Value;
                }
                else
                {
                    logger.Warn($"Failed to retrieve address {address.ToString()}");
                    return int.MaxValue;
                }
            }
        }

        private TimeSpan GetTimeFromDropOff(RouteStop stop)
        {
            var address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;

            if (address.Equals(Order.RequestInfo.DropOffAddress))
            {
                return TimeSpan.Zero;
            }
            else
            {
                if (_dropOffToStop.ContainsKey(address))
                {
                    return _dropOffToStop[address].Routes.First().Legs.First().Duration.Value;
                }
                else
                {
                    logger.Warn($"Failed to retrieve address {address.ToString()}");
                    return TimeSpan.MaxValue;
                }
            }
        }

        private TimeSpan GetTimeFromPickUp(RouteStop stop)
        {
            var address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;

            if (address.Equals(Order.RequestInfo.PickUpAddress))
            {
                return TimeSpan.Zero;
            }
            else
            {
                if (_pickUpToStop.ContainsKey(address))
                {
                    return _pickUpToStop[address].Routes.First().Legs.First().Duration.Value;
                }
                else
                {
                    logger.Warn($"Failed to retrieve address {address.ToString()}");
                    return TimeSpan.MaxValue;
                }

            }
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