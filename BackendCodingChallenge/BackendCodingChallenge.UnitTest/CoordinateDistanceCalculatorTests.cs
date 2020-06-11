using System;
using BackendCodingChallenge.Calculators.CoordinateDistance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendCodingChallenge.UnitTest
{
    [TestClass]
    public class CoordinateDistanceCalculatorTests
    {
        [TestMethod]
        public void ComputeDistance_SameCoordinates_ReturnZero()
        {
            var coordinateDistanceCalculator = new CoordinateDistanceCalculator();
            var distance = coordinateDistanceCalculator.ComputeDistance(33.45, -21.45, 33.45, -21.45);
            
            Assert.AreEqual(0, distance);
        }
        
        [TestMethod]
        public void ComputeDistance_CanadianCitiesMontrealOttawa_Return165()
        {
            //Montreal(Lat:45.50884, Long:-73.58781), Ottawa(Lat:45.41117, Long:-75.69812)
            //Distance between both cities is 165km
            var coordinateDistanceCalculator = new CoordinateDistanceCalculator();
            var distance = coordinateDistanceCalculator.ComputeDistance(45.50884, -73.58781, 45.41117, -75.69812);
            
            Assert.AreEqual(165, Math.Round(distance));
        }
    }
}