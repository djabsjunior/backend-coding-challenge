namespace BackendCodingChallenge.Calculators.CoordinateDistance
{
    public interface ICoordinateDistanceCalculator
    {
        /// <summary>
        /// Compute the distance(km) between the request GeoCoordinate and the current city
        /// </summary>
        /// <param name="reqLatitude"></param>
        /// <param name="reqLongitude"></param>
        /// <param name="cityLatitude"></param>
        /// <param name="cityLongitude"></param>
        /// <returns></returns>
        public double ComputeDistance(double reqLatitude, double reqLongitude, double cityLatitude, double cityLongitude);
    }
}
