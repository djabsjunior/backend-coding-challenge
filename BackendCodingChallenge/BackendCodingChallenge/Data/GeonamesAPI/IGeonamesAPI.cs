using BackendCodingChallenge.Models;

namespace BackendCodingChallenge.Data.GeonamesAPI
{
    public interface IGeonamesApi
    {
        /// <summary>
        /// Get all Cities' starting with 'req' data from api/geonames.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CitiesModel GetCitiesData(string req);
    }
}
