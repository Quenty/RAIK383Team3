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
        private bool _WillWork = true;

        private bool _AlreadyCalculated = false;
        public bool WillWork { get
            {
                if (!_AlreadyCalculated)
                {
                    try
                    {
                        CalculateOptimalRoute();
                    }
                    catch (Exception e)
                    {
                        logger.Error($"AssignOrder() - Error with RoutePlannerDelegate with ContractorInfoGuid {Contractor.ContractorInfoGuid.ToString()} - {e.ToString()}");
                    }
                }

                return _WillWork;
            }
        }
        public ContractorInfo Contractor { get; }
        public Order Order { get; }
        public List<RouteStop> Route { get; }
        public RouteStop PickUp { get; }
        public RouteStop DropOff { get; }
        public int DistanceChanged { get; set; }

        private Dictionary<string, DirectionsResponse> _pickUpToStop = new Dictionary<string, DirectionsResponse>();
        private Dictionary<string, DirectionsResponse> _dropOffToStop = new Dictionary<string, DirectionsResponse>();
        private List<TruckCapacityUnit> _capacityUnits = new List<TruckCapacityUnit>(); //these will be the capacities immediately after the stop corresponding to the index



        public ContractorRoutePlanner(ContractorInfo contractor, Order order, List<RouteStop> route)
        {
            Contractor = contractor;
            Order = order;
            Route = route;

            PickUp = new RouteStop
            {
                RouteStopGuid = Guid.NewGuid(),
                Type = StopType.PickUp
            };

            DropOff = new RouteStop
            {
                RouteStopGuid = Guid.NewGuid(),
                Type = StopType.DropOff
            };

            
        }

        public void CalculateOptimalRoute()
        {

            if (_AlreadyCalculated)
            {
                return;
            }

            _AlreadyCalculated = true;

            if (!IsWithinMaxCapacity(Order.RequestInfo.Size, Contractor.Truck.MaxCapacity))
            {
                _WillWork = false;
                return;
            }


            if (!Route.Any())
            {
                DeliverFromHomeAddress();
                return;
            }
            
            GetTruckCapacityUnits();

            IEnumerable<Tuple<int, int>> sublists = GetSubRoutesWithNoCapacityConflicts();
            GetDistances(sublists);

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




        private void GetDistances(IEnumerable<Tuple<int, int>> sublists)
        {
            var pickUpLocation = Order.RequestInfo.PickUpAddress;
            var dropOffLocation = Order.RequestInfo.DropOffAddress;

            //PickUp to DropOff and vice versa
            var pickUpToDropOff = LocationHelper.GetRouteBetweenLocations(pickUpLocation, dropOffLocation);
            if (pickUpToDropOff.Status == DirectionsStatusCodes.OK)
            {
                _pickUpToStop.Add(dropOffLocation.ToString(), pickUpToDropOff);
                _dropOffToStop.Add(pickUpLocation.ToString(), pickUpToDropOff);
            }
            else
            {
                logger.Warn($"Google Maps Api failed to load address {dropOffLocation}. Error is {pickUpToDropOff.Status}");
            }
           
            foreach(var subroute in sublists)
            {
                //we want the last index for all the subroutes except the last subroute, which is past the last item in routes
                var endIndex = subroute.Equals(sublists.Last()) ? subroute.Item2 - 1 : subroute.Item2;
                for(var i = subroute.Item1; i <= endIndex; i++)
                {
                    var stop = Route.ElementAt(i);
                    var location = (stop.Type == StopType.PickUp) ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;

                    //Add PickUp to location
                    if (!_pickUpToStop.ContainsKey(location.ToString()))
                    {
                        var response = LocationHelper.GetRouteBetweenLocations(pickUpLocation, location);
                        if (response.Status == DirectionsStatusCodes.OK)
                        {
                            _pickUpToStop.Add(location.ToString(), response);
                        }
                        else
                        {
                            logger.Warn($"Google Maps Api failed to load address {location}. Error is {response.Status}");
                        }
                    }

                    //Add DropOffToLocation
                    if (!_dropOffToStop.ContainsKey(location.ToString()))
                    {
                        var response = LocationHelper.GetRouteBetweenLocations(dropOffLocation, location);
                        if (response.Status == DirectionsStatusCodes.OK)
                        {
                            _dropOffToStop.Add(location.ToString(), response);
                        }
                        else
                        {
                            logger.Warn($"Google Maps Api failed to load address {location}. Error is {response.Status}");
                        }
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
            var homeToPickUp = LocationHelper.GetRouteBetweenLocations(Contractor.HomeAddress, Order.RequestInfo.PickUpAddress);
            var pickUpToDropoff = LocationHelper.GetRouteBetweenLocations(Order.RequestInfo.PickUpAddress, Order.RequestInfo.DropOffAddress);
            PickUp.DistanceToNextStop = pickUpToDropoff.Routes.First().Legs.First().Distance.Value;
            PickUp.EstimatedTimeToNextStop = pickUpToDropoff.Routes.First().Legs.First().Duration.Value;

            DistanceChanged = homeToPickUp.Routes.First().Legs.First().Distance.Value + PickUp.DistanceToNextStop;
            Route.Add(PickUp);
            Route.Add(DropOff);

            OrderRouteStops();
        }

        private int GetDistanceFromPickUp(RouteStop stop)
        {
            CivicAddressDb address = null;

            if (stop.Equals(PickUp))
            {
                return 0;
            }
            else if (stop.Equals(DropOff))
            {
                address = Order.RequestInfo.DropOffAddress;
            }
            else
            {
                address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;
            }

            if (_pickUpToStop.ContainsKey(address.ToString()))
            {
                return _pickUpToStop[address.ToString()].Routes.First().Legs.First().Distance.Value;
            }
            else if (address.ToString().Equals(Order.RequestInfo.PickUpAddress.ToString()))
            {
                return 0;
            }
            else
            {
                logger.Warn($"Failed to retrieve address {address.ToString()}");
                return int.MaxValue;
            }
        }

        private int GetDistanceFromDropOff(RouteStop stop)
        {

            CivicAddressDb address = null;

            if (stop.Equals(DropOff))
            {
                return 0;
            }
            else if (stop.Equals(PickUp))
            {
                address = Order.RequestInfo.PickUpAddress;
            }
            else
            {
                address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;
            }

            if (_dropOffToStop.ContainsKey(address.ToString()))
            {
                return _dropOffToStop[address.ToString()].Routes.First().Legs.First().Distance.Value;
            }
            else if (address.ToString().Equals(Order.RequestInfo.DropOffAddress.ToString()))
            {
                return 0;
            }
            else
            {
                logger.Warn($"Failed to retrieve address {address.ToString()}");
                return int.MaxValue;
            }
        }

        private TimeSpan GetTimeFromDropOff(RouteStop stop)
        {
            CivicAddressDb address = null;

            if (stop.Equals(DropOff))
            {
                return TimeSpan.Zero;
            }
            else if (stop.Equals(PickUp))
            {
                address = Order.RequestInfo.PickUpAddress;
            }
            else
            {
                address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;
            }


            if (_dropOffToStop.ContainsKey(address.ToString()))
            {
                return _dropOffToStop[address.ToString()].Routes.First().Legs.First().Duration.Value;
            }
            else if (address.ToString().Equals(Order.RequestInfo.DropOffAddress.ToString()))
            {
                return TimeSpan.Zero;
            }
            else
            {
                logger.Warn($"Failed to retrieve address {address.ToString()}");
                return TimeSpan.MaxValue;
            }
        }

        private TimeSpan GetTimeFromPickUp(RouteStop stop)
        {

            CivicAddressDb address = null;

            if (stop.Equals(PickUp))
            {
                return TimeSpan.Zero;
            }
            else if (stop.Equals(DropOff))
            {
                address = Order.RequestInfo.DropOffAddress;
            }
            else
            {
                address = stop.Type == StopType.PickUp ? stop.Order.RequestInfo.PickUpAddress : stop.Order.RequestInfo.DropOffAddress;
            }


            if (_pickUpToStop.ContainsKey(address.ToString()))
            {
                return _pickUpToStop[address.ToString()].Routes.First().Legs.First().Duration.Value;
            }
            else if (address.ToString().Equals(Order.RequestInfo.PickUpAddress.ToString()))
            {
                return TimeSpan.Zero;
            }
            else
            {
                logger.Warn($"Failed to retrieve address {address.ToString()}");
                return TimeSpan.MaxValue;
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