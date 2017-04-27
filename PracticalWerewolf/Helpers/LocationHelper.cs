using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PracticalWerewolf.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Device.Location;
using System.IO;
using System.Linq;

namespace PracticalWerewolf.Helpers
{
    public class LocationHelper : ILocationHelper
    {
        private static ILog logger = LogManager.GetLogger(typeof(LocationHelper));
        private static readonly string FILE_NAME = Path.Combine(Path.GetTempPath(), "Temporary_PracticalWerewolf.cache");
        private static Dictionary<int, Dictionary<int, DirectionsResult>> DirectionsLookUp;
        private static bool isLoaded = false;

        public LocationHelper()
        {
            LoadLookUpTable();
        }

        public static DbGeography CreatePoint(double? lat, double? lon, int srid = 4326)
        {
            string wkt = String.Format("POINT({0} {1})", lon, lat);

            return DbGeography.PointFromText(wkt, srid);
        }

        public DirectionsResult GetDirections(DbGeography origin, CivicAddressDb destination)
        {
            if (origin == null || destination == null)
            {
                logger.Error("GetDirections() - null argument");
                throw new ArgumentNullException();
            }

            var firstAddress = $"{origin.Latitude}, {origin.Longitude}";

            var response = GetRouteBetweenLocations(firstAddress.ToString(), destination.ToString());
            if (response != null && response.Status == DirectionsStatusCodes.OK)
            {
                return new DirectionsResult
                {
                    Distance = response.Routes.First().Legs.First().Distance.Value,
                    Duration = response.Routes.First().Legs.First().Duration.Value
                };
            }
            else
            {
                logger.Error($"Google Maps Api failed to find route from {origin} to {destination}. Error is {response.StatusStr}");
                throw new ApplicationException();
            }
        }

        public DirectionsResult GetDirections(CivicAddressDb address1, CivicAddressDb address2)
        {
            if(address1 == null || address2 == null)
            {
                logger.Error("GetDirections() - null argument");
                throw new ArgumentNullException();
            }

            if(address1 == address2)
            {
                return new DirectionsResult
                {
                    Origin = address1,
                    Destination = address2,
                    Distance = 0,
                    Duration = TimeSpan.Zero
                };
            }

            DirectionsResult directions = null;

            LoadLookUpTable();

            if (isLoaded)
            {
                directions = GetFromLookUpTable(address1, address2);

                if (directions != null)
                {
                    return directions;
                }
            }

            directions = GetRouteBetweenLocations(address1, address2);
            AddDirectionsToLookUpTable(directions);

            return directions;
        }

        private DirectionsResult GetRouteBetweenLocations(CivicAddressDb origin, CivicAddressDb destination)
        {
            if (origin == null || destination == null)
            {
                //TODO: add possibly valuable info
                logger.Error("GetRouteBetweenLocations(CivicAddressDb, CivicAddressDb) - null argument");
                throw new ArgumentNullException();
            }

            var response = GetRouteBetweenLocations(origin.ToString(), destination.ToString());
            if (response != null &&  response.Status == DirectionsStatusCodes.OK)
            {
                return new DirectionsResult
                {
                    Origin = origin,
                    Destination = destination,
                    Distance = response.Routes.First().Legs.First().Distance.Value,
                    Duration = response.Routes.First().Legs.First().Duration.Value
                };
            }
            else
            {
                logger.Error($"Google Maps Api failed to find route from {origin} to {destination}. Error is {response.StatusStr}");
                throw new ApplicationException();
            }
        }

        private DirectionsResponse GetRouteBetweenLocations(string origin, string destination)
        {
            if (origin == null || destination == null)
            {
                //TODO: add possibly valuable info
                logger.Error("GetRouteBetweenLocations(CivicAddressDb, CivicAddressDb) - null argument");
                throw new ArgumentNullException();
            }

            DirectionsRequest directionsRequest = new DirectionsRequest()
            {
                Origin = origin,
                Destination = destination
            };

            var response = GoogleMaps.Directions.Query(directionsRequest);

            if (response.Status == DirectionsStatusCodes.OK)
            {
                return response;
            }
            else
            {
                logger.Error($"Google Maps Api failed to find route from {origin} to {destination}. Error is {response.StatusStr}");
                return null;
            }
        }

        private void AddDirectionsToLookUpTable(DirectionsResult directions)
        {
            if (isLoaded)
            {
                if (DirectionsLookUp.ContainsKey(directions.Origin.GetHashCode()))
                {
                    DirectionsLookUp[directions.Origin.GetHashCode()].Add(directions.Destination.GetHashCode(), directions);
                }
                else if (DirectionsLookUp.ContainsKey(directions.Destination.GetHashCode()))
                {
                    DirectionsLookUp[directions.Destination.GetHashCode()].Add(directions.Origin.GetHashCode(), directions);
                }
                else
                {
                    var newDict = new Dictionary<int, DirectionsResult>
                    {
                        { directions.Destination.GetHashCode(), directions }
                    };

                    DirectionsLookUp.Add(directions.Origin.GetHashCode(), newDict);
                }
            }
        }

        private DirectionsResult GetFromLookUpTable(CivicAddressDb address1, CivicAddressDb address2)
        {
            try
            {
                if (DirectionsLookUp.ContainsKey(address1.GetHashCode()))
                {
                    var map = DirectionsLookUp[address1.GetHashCode()];

                    if (map.ContainsKey(address2.GetHashCode()))
                    {
                        return map[address2.GetHashCode()];
                    }
                }

                if (DirectionsLookUp.ContainsKey(address2.GetHashCode()))
                {
                    var map = DirectionsLookUp[address2.GetHashCode()];

                    if (map.ContainsKey(address1.GetHashCode()))
                    {
                        return map[address1.GetHashCode()];
                    }
                }
            }
            catch (Exception e)
            {
                logger.Warn("Error reading from LookUp Table", e);
            }
            return null;
        }

        private void LoadLookUpTable()
        {
            if (!isLoaded)
            {
                try
                {
                    DirectionsLookUp = ReadLookUpFromFile();
                    isLoaded = true;
                }
                catch (Exception e)
                {
                    isLoaded = false;
                    logger.Error($"Error reading cached directions from file {FILE_NAME} ", e);
                }
            }
        }

        private Dictionary<int, Dictionary<int, DirectionsResult>> ReadLookUpFromFile()
        {
            if (!File.Exists(FILE_NAME))
            {
                var dict = new Dictionary<int, Dictionary<int, DirectionsResult>>();
                WriteToFile(dict);
                return dict;
            }

            string jsonLookup = File.ReadAllText(FILE_NAME);
            return JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, DirectionsResult>>>(jsonLookup);
        }

        private void WriteToFile(Dictionary<int, Dictionary<int, DirectionsResult>> dict)
        {
            var jsonResult = JsonConvert.SerializeObject(dict);

            using (StreamWriter file = new StreamWriter(FILE_NAME, false))
            {
                file.WriteLine(jsonResult);
            };
        }

        public void Refresh()
        {
            WriteToFile(DirectionsLookUp);
        }
    }
}