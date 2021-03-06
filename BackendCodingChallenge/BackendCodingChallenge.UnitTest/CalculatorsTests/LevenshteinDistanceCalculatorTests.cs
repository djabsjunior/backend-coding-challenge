using BackendCodingChallenge.Calculators.LevenshteinDistance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendCodingChallenge.UnitTest.CalculatorsTests
{
    [TestClass]
    public class LevenshteinDistanceCalculatorTests
    {
        [TestMethod]
        public void ComputeDistance_SameCityName_ReturnZero()
        {
            var levenshteinDistanceCalculator = new LevenshteinDistanceCalculator();
            var distance = levenshteinDistanceCalculator.ComputeDistance("Québec", "Québec");
            
            Assert.AreEqual(0, distance);
        }
        
        [TestMethod]
        public void ComputeDistance_DifferentCityNames_Return8()
        {
            //Montreal and Mont-Tremblant
            var levenshteinDistanceCalculator = new LevenshteinDistanceCalculator();
            var distance = levenshteinDistanceCalculator.ComputeDistance("Montréal", "Mont-Tremblant");
            
            Assert.AreEqual(8, distance);
        }
    }
}