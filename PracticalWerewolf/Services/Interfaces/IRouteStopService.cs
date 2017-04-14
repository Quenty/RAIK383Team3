using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Models.UserInfos;
using System.Collections.Generic;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IRouteStopService
    {
        //Retreives the RouteStops for the contractor that are in progress or queued
        IEnumerable<RouteStop> GetContractorRoute(ContractorInfo contractor);

        //retrieves the current in progress routeStop for the contractor
        RouteStop GetContractorCurrentAssignment(ContractorInfo contractor);

    }
}