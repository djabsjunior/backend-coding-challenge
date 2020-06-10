using System;
using System.IO;
using System.Net;
using BackendCodingChallenge.Models;
using Newtonsoft.Json;

namespace BackendCodingChallenge.Data.GeonamesAPI
{
    public class GeonamesAPI : IGeonamesAPI
    {
        public CitiesModel GetCitiesData(string req)
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
    }
}
