using System;
using System.Device.Location;

namespace PracticalWerewolf.Models
{
    public interface IOrder
    {
        IOrderRequestInfo RequestInfo { get; set; }
        IOrderTrackInfo TrackInfo { get; set; }
    }

    public interface IOrderRequestInfo
    {
        ICustomerInfo Requester { get; set; }
        ITruckCapacityUnit Size { get; set; }
        // TODO: Implement size

        DateTime RequestDate { get; set; }
        CivicAddress PickUpAddress { get; set; }
        CivicAddress DropOffAddress { get; set; }
    }

    public interface IOrderTrackInfo
    {
        ITruck CurrentTruck { get; set; }
        IContractorInfo Assignee { get; set; }
    }
}
