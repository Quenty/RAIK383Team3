using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Controllers
{
    public class LocationHelper
    {
        public static DbGeography CreatePoint(double lat, double lon, int srid = 4326)
        {
            string wkt = String.Format("POINT({0} {1})", lon, lat);

            return DbGeography.PointFromText(wkt, srid);
        }

        public static DirectionsResponse GetRouteBetweenLocations(string origin, string destination)
        {
            DirectionsRequest directionsRequest = new DirectionsRequest()
            {
                Origin = origin,
                Destination = destination
            };

            //There are many errors that will need to be handled higher in the call stack
            //Checkt them with DirectionResponse.Status
            return GoogleMaps.Directions.Query(directionsRequest);
        }

        public static DirectionsResponse GetRouteBetweenLocations(CivicAddressDb origin, CivicAddressDb destination)
        {
            return GetRouteBetweenLocations(origin, destination);
        }
    }
}