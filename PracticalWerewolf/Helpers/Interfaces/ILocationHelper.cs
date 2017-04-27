using GoogleMapsApi.Entities.Directions.Response;
using System.Data.Entity.Spatial;
using System.Device.Location;

namespace PracticalWerewolf.Helpers.Interfaces
{
    public interface ILocationHelper
    {
        DirectionsResult GetDirections(DbGeography origin, CivicAddressDb destination);
        DirectionsResult GetDirections(CivicAddressDb address1, CivicAddressDb address2);
        void Refresh();
    }
}
