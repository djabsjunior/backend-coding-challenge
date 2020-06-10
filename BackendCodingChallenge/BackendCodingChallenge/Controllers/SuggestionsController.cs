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
    public class SuggestionsController : Controller
    {
        [HttpGet]
        public IActionResult Get([FromQuery] SuggestionsParametersModel parameters)
        {
            var suggestionModel = new SuggestionsModel();

            if (!SuggestionsParametersModelIsValid(parameters))
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid parameters. 'q' must be a string, 'longitude' and 'latitude' values must be numbers.");
            }

            suggestionModel.Suggestions = GetSuggestions(parameters);

            return Ok(suggestionModel);
        }

        /// <summary>
        /// Validate query parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private bool SuggestionsParametersModelIsValid(SuggestionsParametersModel parameters)
        {
            parameters.Latitude ??= "0";
            parameters.Longitude ??= "0";

            return !string.IsNullOrWhiteSpace(parameters.Q)
                && double.TryParse(parameters.Latitude, out _)
                && double.TryParse(parameters.Latitude, out _);
        }

        /// <summary>
        /// Returns SuggestionModel based on api/Geonames results.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        private List<Suggestion> GetSuggestions(SuggestionsParametersModel parameters)
        {
            var suggestionList = new List<Suggestion>();

            if (string.IsNullOrWhiteSpace(parameters.Q))
            {
                return suggestionList;
            }

            var citiesModel = GetCities(parameters.Q);
            var citiesScores = GetCitiesScores(citiesModel, parameters);

            foreach (var city in citiesModel.Cities.Where(c => citiesScores.Exists(cs => cs.Key == c.CityId)))
            {
                string[] cityNameArray = { city.Name, city.AdministrationCodes.ProvinceStateCode, city.CountryCode };

                suggestionList.Add(new Suggestion
                {
                        Latitude = city.Latitude,
                        Longitude = city.Longitude,
                        Name = string.Join(", ", cityNameArray),
                        Score = citiesScores.FirstOrDefault(cs => cs.Key == city.CityId).Value
                });
            }

            return suggestionList.OrderByDescending(s => s.Score).ToList();
        }

        /// <summary>
        /// GET from api/geonames the list (medium style) of cities names starting with the string request.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private CitiesModel GetCities(string req)
        {
            var geonamesRequestUri = string.Format(@"http://api.geonames.org/searchJSON?name_startsWith={0}&cities=cities5000&maxRows=10&country=US&country=CA&style=MEDIUM&username=jbvouma", req);
            var geonamesWebReq = (HttpWebRequest)WebRequest.Create(geonamesRequestUri);

            geonamesWebReq.Method = "GET";
            CitiesModel cities;

            try
            {
                HttpWebResponse geonamesWebResp = (HttpWebResponse)geonamesWebReq.GetResponse();
                string jsonString;

                using (var stream = geonamesWebResp.GetResponseStream())
                {
                    var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    jsonString = reader.ReadToEnd();
                }

                cities = JsonConvert.DeserializeObject<CitiesModel>(jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured when calling api/geonames, exception detail:" + ex);
            }

            return cities;
        }

        /// <summary>
        /// Compute the distance(km) between the request GeoCoordinate and the current city
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

            return requestCoordinate.GetDistanceTo(cityCoordinate) / 1000;
        }

        /// <summary>
        /// Levenshtein distance algorithm to compute distance between the request string and the current city name.
        /// Reference: https://en.wikipedia.org/wiki/Levenshtein_distance, https://www.dotnetperls.com/levenshtein
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        private int GetLevenshteinDistance(string req, string cityName)
        {
            var reqLength = req.Length;
            var cityNameLength = cityName.Length;
            int[,] d = new int[reqLength + 1, cityNameLength + 1];

            // Step 1
            if (reqLength == 0 || cityNameLength == 0)
            {
                return Math.Min(cityNameLength, reqLength);
            }

            // Step 2
            for (var i = 0; i <= reqLength; d[i, 0] = i++)
            {
            }

            for (var j = 0; j <= cityNameLength; d[0, j] = j++)
            {
            }

            // Step 3
            for (var i = 1; i <= reqLength; i++)
            {
                //Step 4
                for (var j = 1; j <= cityNameLength; j++)
                {
                    // Step 5
                    var cost = (cityName[j - 1] == req[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[reqLength, cityNameLength];
        }

        /// <summary>
        /// Returns the score between 0 and 0.5 using the BBF algorithm
        /// Ref: https://www.cs.ubc.ca/~lowe/papers/ijcv04.pdf
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        private List<KeyValuePair<int, double>> GetCitiesScores(CitiesModel citiesModel, SuggestionsParametersModel parametersModel)
        {
            var citiesGeoCoordDistances = citiesModel.Cities.Select(city => new KeyValuePair<int, double>(city.CityId, GetCoordinateDistance(double.Parse(parametersModel.Latitude), double.Parse(parametersModel.Longitude), double.Parse(city.Latitude), double.Parse(city.Longitude)))).OrderBy(c => c.Value).ToList();
            var citiesNamesDistances = citiesModel.Cities.Select(city => new KeyValuePair<int, double>(city.CityId, GetLevenshteinDistance(parametersModel.Q, city.Name))).OrderBy(c => c.Value).ToList();
            var citiesScores = new List<KeyValuePair<int, double>>();

            if(string.Equals(parametersModel.Latitude,"0") && string.Equals(parametersModel.Longitude, "0"))
            {
                return GetScores(citiesNamesDistances, 15, 1);
            }

            var coordScores = GetScores(citiesGeoCoordDistances, 1000, 2);
            var namesScores = GetScores(citiesNamesDistances, 15, 2);

            foreach (var item in coordScores)
            {
                citiesScores.Add(new KeyValuePair<int, double>(item.Key, Math.Round(item.Value + namesScores.FirstOrDefault(n => n.Key == item.Key).Value, 1)));
            }

            return citiesScores;
        }

        /// <summary>
        /// Algorithm to calculate scores based on a  list of distances, a divisor and a base value.
        /// </summary>
        /// <param name="citiesDistances"></param>
        /// <param name="divisor"></param>
        /// <param name="algoBase"></param>
        /// <returns></returns>
        private List<KeyValuePair<int, double>> GetScores(List<KeyValuePair<int, double>> citiesDistances, int divisor, int algoBase)
        {
            var scores = new List<KeyValuePair<int, double>>();

            for (int i = 0; i < citiesDistances.Count; i++)
            {
                var score = Math.Exp(-(citiesDistances[i].Value / divisor)) / algoBase;
                scores.Add(new KeyValuePair<int, double>(citiesDistances[i].Key, algoBase == 1 ? Math.Round(score,1) : score));
            }

            return scores;
        }
    }
}
