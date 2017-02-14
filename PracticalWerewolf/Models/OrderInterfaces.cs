using System;

namespace PracticalWerewolf.Models
{
    public interface IOrder
    {
        IOrderRequestInfo RequestInfo { get; set; }
        IOrderTrackInfo TrackInfo { get; set; }
    }

    public interface IOrderRequestInfo
    {

    }

    public interface IOrderTrackInfo
    {

    }


}