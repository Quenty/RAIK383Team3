﻿using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Models.UserInfos;
using System;
using System.Collections.Generic;
using PracticalWerewolf.Models.Orders;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IRouteStopService
    {
        //Retreives the RouteStops for the contractor that are in progress or queued
        IEnumerable<RouteStop> GetContractorRouteAsNoTracking(ContractorInfo contractor);

        //retrieves the current in progress routeStop for the contractor
        RouteStop GetContractorCurrentAssignment(ContractorInfo contractor);

        IEnumerable<RouteStop> GetContractorRoute(ContractorInfo contractor);

        string GetDistanceToNextStopInMiles(ContractorInfo contractor);
     
        void UnassignOrderFromRouteStop(Order order);

        void Update(RouteStop entity);

        void Update(IEnumerable<RouteStop> entities);

        void Insert(RouteStop entity);

        void Attach(RouteStop entity);

        void Attach(IEnumerable<RouteStop> entities);

        RouteStop GetRouteStop(Guid guid);
    }
}