using System;
using BackendCodingChallenge.Calculators.CoordinateDistance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendCodingChallenge.UnitTest.CalculatorsTests
{
    [TestClass]
    public class CoordinateDistanceCalculatorTests
    {
        [TestMethod]
        public void ComputeDistance_SameCoordinates_ReturnZero()
        {
            var coordinateDistanceCalculator = new CoordinateDistanceCalculator();
            var distance = coordinateDistanceCalculator.ComputeDistance((decimal) 33.45, (decimal) -21.45, (decimal) 33.45, (decimal) -21.45);
            
            Assert.AreEqual(0, distance);
        }
        
        [TestMethod]
        public void ComputeDistance_CanadianCitiesMontrealOttawa_Return165()
        {
            //Montreal(Lat:45.50884, Long:-73.58781), Ottawa(Lat:45.41117, Long:-75.69812)
            //Distance between both cities is 165km
            var coordinateDistanceCalculator = new CoordinateDistanceCalculator();
            var distance = coordinateDistanceCalculator.ComputeDistance((decimal) 45.50884, (decimal) -73.58781, (decimal) 45.41117, (decimal) -75.69812);
            
            Assert.AreEqual(165, Math.Round(distance));
        }
    }
}