using BackendCodingChallenge.Models;
using BackendCodingChallenge.Validations.ParametersValidation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendCodingChallenge.UnitTest
{
    [TestClass]
    public class SuggestionsParametersTest
    {
        [TestMethod]
        public void IsValid_NullQParameter_ReturnFalse()
        {
            var suggestionsParameters = new SuggestionsParameters();
            var isValid = suggestionsParameters.IsValid(new SuggestionsParametersModel() { Latitude = "39.89", Longitude = "-55.34" });

            Assert.IsFalse(isValid);
        }
        
        [TestMethod]
        public void IsValid_LatitudeNotNumber_ReturnFalse()
        {
            var suggestionsParameters = new SuggestionsParameters();
            var isValid = suggestionsParameters.IsValid(new SuggestionsParametersModel() { Q = "Van", Latitude = "kj", Longitude = "-55.34" });

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsValid_WithoutCoordinates_ReturnTrue()
        {
            var suggestionsParameters = new SuggestionsParameters();
            var isValid = suggestionsParameters.IsValid(new SuggestionsParametersModel() { Q = "Montreal" });

            Assert.IsTrue(isValid);
        }
    }
}
