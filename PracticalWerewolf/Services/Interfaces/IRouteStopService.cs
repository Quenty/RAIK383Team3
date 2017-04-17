using PracticalWerewolf.Models.Routes;
using PracticalWerewolf.Models.UserInfos;
using System.Collections.Generic;

namespace PracticalWerewolf.Services.Interfaces
{
    public interface IRouteStopService
    {
        IEnumerable<RouteStop> GetContractorRoute(ContractorInfo contractor);
        RouteStop GetContractorCurrentAssignment(ContractorInfo contractor);
    }
}