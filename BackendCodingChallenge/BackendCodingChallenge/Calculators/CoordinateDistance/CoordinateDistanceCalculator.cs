using System.Device.Location;

namespace BackendCodingChallenge.Calculators.CoordinateDistance
{
    public class CoordinateDistanceCalculator : ICoordinateDistanceCalculator
    {
        public double ComputeDistance(double reqLatitude, double reqLongitude, double cityLatitude, double cityLongitude)
        {
            var requestCoordinate = new GeoCoordinate(reqLatitude, reqLongitude);
            var cityCoordinate = new GeoCoordinate(cityLatitude, cityLongitude);

            return requestCoordinate.GetDistanceTo(cityCoordinate) / 1000;
        }
    }
}
