using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using BackendCodingChallenge.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BackendCodingChallenge.Controllers
{
    [Produces("application/json")]
    [Route("/suggestions")]
    public class SuggestionController : Controller
    {
        [HttpGet]
        public IActionResult Get(string q, string latitude, string longitude)
        {
            var suggestionModel = new SuggestionModel();

            if (string.IsNullOrWhiteSpace(latitude))
            {
                latitude = "0";
            }

            if (string.IsNullOrWhiteSpace(longitude))
            {
                longitude = "0";
            }

            if (!double.TryParse(latitude, out _) || !double.TryParse(longitude, out _))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Longitude and Latitude values must be numbers.");
            }

            suggestionModel.Suggestions = GetSuggestions(q, double.Parse(latitude), double.Parse(longitude));

            return Ok(suggestionModel);
        }

        /// <summary>
        /// Returns SuggestionModel based on api/Geonames results.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private List<Suggestion> GetSuggestions(string req, double reqLatitude, double reqLongitude)
        {
            var suggestionList = new List<Suggestion>();

            if (string.IsNullOrWhiteSpace(req))
            {
                return suggestionList;
            }

            var geonameItems = GetGeonames(req);

            foreach (var city in geonameItems.Geonames)
            {
                string[] cityNameArray = { city.Name, city.AdministrationCodes.ProvinceStateCode, city.CountryCode };
                var cityCoordinateDistance = GetCoordinateDistance(reqLatitude, reqLongitude, double.Parse(city.Latitude), double.Parse(city.Longitude));
                var cityNameLevenshteinDistance = LevenshteinDistance(req, city.Name);

                suggestionList.Add(new Suggestion
                {
                        Latitude = city.Latitude,
                        Longitude = city.Longitude,
                        Name = string.Join(", ", cityNameArray),
                        Score = GetScore(cityCoordinateDistance, cityNameLevenshteinDistance)
                });
            }

            return suggestionList;
        }

        /// <summary>
        /// GET from api/geonames the list (medium style) of cities names starting with the string request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private GeonameModel GetGeonames(string req)
        {
            var geonameItems = new GeonameModel();
            var geonamesRequestUri = string.Format(@"http://api.geonames.org/searchJSON?name_startsWith={0}&cities=cities5000&maxRows=10&country=US&country=CA&style=MEDIUM&username=jbvouma", req);
            var geonamesWebReq = (HttpWebRequest)WebRequest.Create(geonamesRequestUri);

            geonamesWebReq.Method = "GET";

            try
            {
                HttpWebResponse geonamesWebResp = (HttpWebResponse)geonamesWebReq.GetResponse();
                string jsonString;

                using (var stream = geonamesWebResp.GetResponseStream())
                {
                    var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    jsonString = reader.ReadToEnd();
                }

                geonameItems = JsonConvert.DeserializeObject<GeonameModel>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return geonameItems;
        }

        /// <summary>
        /// Algorithm to compute score based on GeoCoordinate and Levenshtein distances
        /// </summary>
        /// <param name="coordinateDistance"></param>
        /// <param name="LevenshteinDistance"></param>
        /// <returns></returns>
        private double GetScore(double coordinateDistance, double LevenshteinDistance)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compute the distance between the request GeoCoordinate and the current city
        /// </summary>
        /// <param name="reqLatitude"></param>
        /// <param name="reqLongitude"></param>
        /// <param name="cityLatitude"></param>
        /// <param name="cityLongitude"></param>
        /// <returns></returns>
        private double GetCoordinateDistance(double reqLatitude, double reqLongitude, double cityLatitude, double cityLongitude)
        {  
            var requestCoordinate = new GeoCoordinate(reqLatitude, reqLongitude);
            var cityCoordinate = new GeoCoordinate(cityLatitude, cityLongitude);

            return requestCoordinate.GetDistanceTo(cityCoordinate) / 1000.0;
        }

        /// <summary>
        /// Levenshtein distance algorithm to compute distance between the request string and the current city name.
        /// Reference: https://en.wikipedia.org/wiki/Levenshtein_distance, https://www.dotnetperls.com/levenshtein
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        private int LevenshteinDistance(string req, string cityName)
        {
            int reqLength = req.Length;
            int cityNameLength = cityName.Length;
            int[,] d = new int[reqLength + 1, cityNameLength + 1];

            // Step 1
            if (reqLength == 0)
            {
                return cityNameLength;
            }

            if (cityNameLength == 0)
            {
                return reqLength;
            }

            // Step 2
            for (int i = 0; i <= reqLength; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= cityNameLength; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= reqLength; i++)
            {
                //Step 4
                for (int j = 1; j <= cityNameLength; j++)
                {
                    // Step 5
                    int cost = (cityName[j - 1] == req[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[reqLength, cityNameLength];
        }
    }
}
