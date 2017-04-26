using GoogleMapsApi.Entities.Directions.Response;
using System.Device.Location;

namespace PracticalWerewolf.Helpers.Interfaces
{
    public interface ILocationHelper
    {
        DirectionsResult GetDirections(CivicAddressDb address1, CivicAddressDb address2);
        void Refresh();
    }
}
