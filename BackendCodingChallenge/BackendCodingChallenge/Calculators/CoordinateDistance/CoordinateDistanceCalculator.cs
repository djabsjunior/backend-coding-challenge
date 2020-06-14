using System.Device.Location;

namespace BackendCodingChallenge.Calculators.CoordinateDistance
{
    public class CoordinateDistanceCalculator : ICoordinateDistanceCalculator
    {
        public decimal ComputeDistance(decimal reqLatitude, decimal reqLongitude, decimal cityLatitude, decimal cityLongitude)
        {
            var requestCoordinate = new GeoCoordinate(decimal.ToDouble(reqLatitude), decimal.ToDouble(reqLongitude));
            var cityCoordinate = new GeoCoordinate(decimal.ToDouble(cityLatitude), decimal.ToDouble(cityLongitude));

            return (decimal)requestCoordinate.GetDistanceTo(cityCoordinate) / 1000;
        }
    }
}
