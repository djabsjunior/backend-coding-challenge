using System;
using System.IO;
using System.Net;
using BackendCodingChallenge.Models;
using Newtonsoft.Json;

namespace BackendCodingChallenge.Data.GeonamesAPI
{
    public class GeonamesApi : IGeonamesApi
    {
        public CitiesModel GetCitiesData(string req)
        {
            var geonamesRequestUri = $@"http://api.geonames.org/searchJSON?name_startsWith={req}&cities=cities5000&maxRows=10&country=US&country=CA&style=MEDIUM&username=jbvouma";
            var geonamesWebReq = (HttpWebRequest)WebRequest.Create(geonamesRequestUri);

            geonamesWebReq.Method = "GET";
            CitiesModel cities;

            try
            {
                var geonamesWebResp = (HttpWebResponse)geonamesWebReq.GetResponse();
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
                throw new Exception("An error occured when calling the Geonames API, exception detail: " + ex);
            }

            return cities;
        }
    }
}
