using System.Collections.Generic;
using Newtonsoft.Json;

namespace BackendCodingChallenge.Models
{
    public partial class CitiesModel
    {
        [JsonProperty("totalResultsCount")]
        public int TotalResultsCount { get; set; }

        [JsonProperty("geonames")]
        public List<City> Cities { get; set; }
    }

    public partial class City
    {
        [JsonProperty("adminCode1")]
        public string AdministrationCode { get; set; }

        [JsonProperty("lng")]
        public string Longitude { get; set; }

        [JsonProperty("geonameId")]
        public int CityId { get; set; }

        [JsonProperty("toponymName")]
        public string ToponymName { get; set; }

        [JsonProperty("countryId")]
        public string CountryId { get; set; }

        [JsonProperty("fcl")]
        public string FeatureClass { get; set; }

        [JsonProperty("population")]
        public int Population { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fclName")]
        public string FeatureClassName { get; set; }

        [JsonProperty("adminCodes1")]
        public AdministrationCodes AdministrationCodes { get; set; }

        [JsonProperty("countryName")]
        public string CountryName { get; set; }

        [JsonProperty("fcodeName")]
        public string FeatureCodeName { get; set; }

        [JsonProperty("adminName1")]
        public string AdministrationFullName { get; set; }

        [JsonProperty("lat")]
        public string Latitude { get; set; }

        [JsonProperty("fcode")]
        public string FeatureCode { get; set; }
    }

    public partial class AdministrationCodes
    {
        [JsonProperty("ISO3166_2")]
        public string ProvinceStateCode { get; set; }
    }
}
