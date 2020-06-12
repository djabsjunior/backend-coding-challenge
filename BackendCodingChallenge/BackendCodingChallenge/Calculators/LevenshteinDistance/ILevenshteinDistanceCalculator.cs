namespace BackendCodingChallenge.Calculators.LevenshteinDistance
{
    public interface ILevenshteinDistanceCalculator
    {
        /// <summary>
        /// Levenshtein distance algorithm to compute distance between the request string and the current city name.
        /// Reference: https://en.wikipedia.org/wiki/Levenshtein_distance
        /// </summary>
        /// <param name="req"></param>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public int ComputeDistance(string req, string cityName);
    }
}
