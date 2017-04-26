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
using PracticalWerewolf.Helpers.Interfaces;

namespace PracticalWerewolf.Services
{
    public class ContractorRoutePlanner
    {
        private static ILog logger = LogManager.GetLogger(typeof(ContractorRoutePlanner));
        public static ILocationHelper LocationHelper;
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

        private List<TruckCapacityUnit> _capacityUnits = new List<TruckCapacityUnit>(); //these will be the capacities immediately after the stop corresponding to the index

        public ContractorRoutePlanner(ContractorInfo contractor, Order order, List<RouteStop> route, ILocationHelper locationHelper)
        {
            LocationHelper = locationHelper;
            Contractor = contractor;
            Order = order;
            Route = route;

            PickUp = new RouteStop
            {
                RouteStopGuid = Guid.NewGuid(),
                Type = StopType.PickUp,
                Order = order
            };

            DropOff = new RouteStop
            {
                RouteStopGuid = Guid.NewGuid(),
                Type = StopType.DropOff,
                Order = order
            };
        }

        public void CalculateOptimalRoute()
        {

            if (_AlreadyCalculated)
            {
                return;
            }

            _AlreadyCalculated = true;

            if (!Order.RequestInfo.Size.FitsIn(Contractor.Truck.MaxCapacity))
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
            //GetDistances(sublists);

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
            var prevToPickUpDirections = LocationHelper.GetDirections(prev.Address, PickUp.Address);
            prev.DistanceToNextStop = prevToPickUpDirections.Distance;
            prev.EstimatedTimeToNextStop = prevToPickUpDirections.Duration;

            //set PickUp numbers
            var next = Route[best.PickUpIndex + 1];
            var pickUpToNextDirections = LocationHelper.GetDirections(PickUp.Address, next.Address);
            PickUp.DistanceToNextStop = pickUpToNextDirections.Distance;
            PickUp.EstimatedTimeToNextStop = pickUpToNextDirections.Duration;

            //update distance to DropOff here in case we have to insert PickUp first
            //not DropOffIndex-1 because we inserted PickUp before it
            prev = Route[best.DropOffIndex];
            var prevToDropOffDirections = LocationHelper.GetDirections(prev.Address, DropOff.Address);
            prev.DistanceToNextStop = prevToDropOffDirections.Distance;
            prev.EstimatedTimeToNextStop = prevToDropOffDirections.Duration;

            //update DropOff index if it's not the last one
            if (!Route.Last().Equals(DropOff))
            {
                int nextIndex = Route.IndexOf(DropOff) + 1;
                next = Route[nextIndex];
                var dropOffToNextDirections = LocationHelper.GetDirections(DropOff.Address, next.Address);
                DropOff.DistanceToNextStop = dropOffToNextDirections.Distance;
                DropOff.EstimatedTimeToNextStop = dropOffToNextDirections.Duration;
            }

            SetRouteStopOrderIndex();
        }


        private void GetTruckCapacityUnits()
        {
            TruckCapacityUnit current = Contractor.Truck.UsedCapacity;

            foreach(var stop in Route)
            {
                TruckCapacityUnit newCapacity;
                if(stop.Type == StopType.PickUp)
                {
                    newCapacity = current + stop.Order.RequestInfo.Size;
                }
                else
                {
                    newCapacity = current - stop.Order.RequestInfo.Size;
                }

                _capacityUnits.Add(newCapacity);

                current = newCapacity;
            }
        }


        private IEnumerable<Tuple<int, int>> GetSubRoutesWithNoCapacityConflicts()
        {
            List<Tuple<int, int>> sublists = new List<Tuple<int, int>>();
            int count = Route.Count;
            TruckCapacityUnit orderSize = Order.RequestInfo.Size;
            int lastFirstSublistStop = 0;

            for(int i = 0; i < count; i++)
            {
                TruckCapacityUnit capacityWithPickUp = _capacityUnits[i] + orderSize;

                if (!capacityWithPickUp.FitsIn(Contractor.Truck.MaxCapacity))
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
                    var distanceToPickUp = LocationHelper.GetDirections(prev.Address, PickUp.Address).Distance; 
                    totalDistance += distanceToPickUp;

                    previousDistance += prev.DistanceToNextStop;

                    if (j < Route.Count)
                    {
                        var next = Route[j];
                        var distanceFromDropOff = LocationHelper.GetDirections(next.Address, DropOff.Address).Distance; 
                        totalDistance += distanceFromDropOff;
                    }

                    if (i == j)
                    {
                        //if pick up and drop off happen between the same two stops
                        var pickUpToDropOffDistance = LocationHelper.GetDirections(PickUp.Address, DropOff.Address).Distance; 
                        totalDistance += pickUpToDropOffDistance;
                    }
                    else
                    {
                        var afterPickUp = Route[i];
                        var afterPickUpDistance = LocationHelper.GetDirections(PickUp.Address, afterPickUp.Address).Distance;
                        totalDistance += afterPickUpDistance;

                        var beforeDropOff = Route[j - 1];
                        var beforeDropOffDistance = LocationHelper.GetDirections(beforeDropOff.Address, DropOff.Address).Distance; 
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
            var endToPickUp = LocationHelper.GetDirections(end.Address, PickUp.Address);
            var pickUpToDropOff = LocationHelper.GetDirections(PickUp.Address, DropOff.Address);

            return new SubRouteCalculationResult()
            {
                DistanceChanged = endToPickUp.Distance + pickUpToDropOff.Distance,
                DropOffIndex = Route.Count,
                PickUpIndex = Route.Count
            };
        }

        private void DeliverFromHomeAddress()
        {
            var homeToPickUp = LocationHelper.GetDirections(Contractor.HomeAddress, Order.RequestInfo.PickUpAddress);
            var pickUpToDropoff = LocationHelper.GetDirections(Order.RequestInfo.PickUpAddress, Order.RequestInfo.DropOffAddress);
            PickUp.DistanceToNextStop = pickUpToDropoff.Distance;
            PickUp.EstimatedTimeToNextStop = pickUpToDropoff.Duration;

            DistanceChanged = homeToPickUp.Distance + PickUp.DistanceToNextStop;
            Route.Add(PickUp);
            Route.Add(DropOff);

            SetRouteStopOrderIndex();
        }

        private void SetRouteStopOrderIndex()
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