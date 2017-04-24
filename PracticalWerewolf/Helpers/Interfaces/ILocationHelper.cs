using GoogleMapsApi.Entities.Directions.Response;
using System.Device.Location;

namespace PracticalWerewolf.Helpers.Interfaces
{
    public interface ILocationHelper
    {
        DirectionsResult GetRouteBetweenLocations(string origin, string destination);
        DirectionsResult GetRouteBetweenLocations(CivicAddressDb origin, CivicAddressDb destination);
    }
}
