using System.Collections.Generic;
using System.Linq;
using BackendCodingChallenge.Calculators.CoordinateDistance;
using BackendCodingChallenge.Calculators.LevenshteinDistance;
using BackendCodingChallenge.Calculators.Scores;
using BackendCodingChallenge.Data.GeonamesAPI;
using BackendCodingChallenge.Models;
using BackendCodingChallenge.Providers.Suggestions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendCodingChallenge.UnitTest.ProvidersTests
{
    [TestClass]    
    public class SuggestionsDataProviderTests
    {
        [TestMethod]
        public void GetSuggestionsData_NullParameters_ReturnEmptyListOfSuggestions()
        {
            var suggestionsDataProvider = new SuggestionsDataProvider();
            var suggestions = suggestionsDataProvider.GetData(new SuggestionsParametersModel());
            
            CollectionAssert.AreEqual(new List<Suggestion>(), suggestions);
        }
        
        [TestMethod]
        public void GetSuggestions_GivenValuesForAllParameters_ReturnsTrue()
        {
            var suggestionsDataProvider = new SuggestionsDataProvider(new GeonamesApi(),
                new ScoresCalculator(new CoordinateDistanceCalculator(), new LevenshteinDistanceCalculator()));
            var suggestionsParametersModel = new SuggestionsParametersModel()
            {
                Q = "London",
                Latitude = "43.70011",
                Longitude = "-79.4163"
            };
            var actual = suggestionsDataProvider.GetData(suggestionsParametersModel);
            var expected = new List<Suggestion>
            {
                new Suggestion {Latitude = "42.98339", Longitude = "-81.23304", Name = "London, ON, CA", Score = (decimal) 0.9},
                new Suggestion {Latitude = "39.88645", Longitude = "-83.44825", Name = "London, OH, US", Score = (decimal) 0.5},
                new Suggestion {Latitude = "37.12898", Longitude = "-84.08326", Name = "London, KY, US", Score = (decimal) 0.5},
                new Suggestion {Latitude = "42.86509", Longitude = "-71.37395", Name = "Londonderry, NH, US", Score = (decimal) 0.2},
                new Suggestion {Latitude = "38.93345", Longitude = "-76.54941", Name = "Londontowne, MD, US", Score = (decimal) 0.2}
            }.OrderByDescending(s => s.Score).ToList();

            for (var i = 0; i < actual.Count; i++)
            {
                var exp = expected[i];
                var act = actual[i];

                Assert.IsTrue(string.Equals(exp.Latitude, act.Latitude) 
                              && string.Equals(exp.Longitude, act.Longitude) 
                              && string.Equals(exp.Name, act.Name) 
                              && exp.Score == act.Score);
            }
        }
    }
}