using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Web;

namespace PracticalWerewolf.Helpers
{
    public class LocationHelper
    {
        private static ILog logger = LogManager.GetLogger(typeof(LocationHelper));

        public static DbGeography CreatePoint(double? lat, double? lon, int srid = 4326)
        {
            string wkt = String.Format("POINT({0} {1})", lon, lat);

            return DbGeography.PointFromText(wkt, srid);
        }

        public static DirectionsResponse GetRouteBetweenLocations(string origin, string destination)
        {
            if(origin == null || destination == null)
            {
                //TODO: add possibly valuable info
                logger.Error("GetRouteBetweenLocations(string, string) - null argument");
                throw new ArgumentNullException();
            }

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
            if (origin == null || destination == null)
            {
                //TODO: add possibly valuable info
                logger.Error("GetRouteBetweenLocations(CivicAddressDb, CivicAddressDb) - null argument");
                throw new ArgumentNullException();
            }

            string originAddress = GetStringAddress(origin);
            string destinationAddress = GetStringAddress(destination);

            return GetRouteBetweenLocations(originAddress, destinationAddress);
        }

        private static string GetStringAddress(CivicAddressDb address)
        {
            if (String.IsNullOrEmpty(address.RawInputAddress))
            {
                string cityState = string.Join(", ", address.City, address.State);
                string zipCountry = string.Join(", ", address.ZipCode, address.Country);
                string value = string.Join(" ", address.StreetNumber, address.Route, cityState, zipCountry);

                return value;
            }
            else
            {
                return address.RawInputAddress;
            }
        }
    }
}