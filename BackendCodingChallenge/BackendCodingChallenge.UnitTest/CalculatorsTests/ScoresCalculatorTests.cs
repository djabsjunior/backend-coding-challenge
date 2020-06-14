using System.Collections.Generic;
using BackendCodingChallenge.Calculators.CoordinateDistance;
using BackendCodingChallenge.Calculators.LevenshteinDistance;
using BackendCodingChallenge.Calculators.Scores;
using BackendCodingChallenge.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendCodingChallenge.UnitTest.CalculatorsTests
{
    [TestClass]
    public class ScoresCalculatorTests
    {
        [TestMethod]
        public void GetCitiesScores_EmptyCitiesModel_ReturnEmptyListOfKeyValuePair()
        {
            var scoresCalculator = new ScoresCalculator(new CoordinateDistanceCalculator(), new LevenshteinDistanceCalculator());
            var citiesModel = new CitiesModel()
            {
                TotalResultsCount = 0,
                Cities = new List<City>()
            };
            var citiesScores = scoresCalculator.GetCitiesScores(citiesModel, new SuggestionsParametersModel(){Q = "Calg"});
            
            CollectionAssert.AreEqual(new List<KeyValuePair<int, double>>(), citiesScores);    
        }
        
        [TestMethod]
        public void GetCitiesScores_GivenValuesForAllParameters()
        {
            var scoresCalculator = new ScoresCalculator( new CoordinateDistanceCalculator(), new LevenshteinDistanceCalculator());
            var cityModel = new CitiesModel()
            {
                TotalResultsCount = 0,
             
                Cities = new List<City>()
                {
                    new City()
                    {
                        AdministrationCode = "OH",
                        Longitude = "-83.44825",
                        CityId = 4517009,
                        ToponymName = "London",
                        CountryId = "6252001",
                        FeatureClass = "P",
                        Population = 10060,
                        CountryCode = "US",
                        Name = "London",
                        FeatureClassName = "city, village,...",
                        AdministrationCodes = new AdministrationCodes() { ProvinceStateCode = "OH" },
                        CountryName = "United States",
                        FeatureCodeName = "seat of a second-order administrative division",
                        AdministrationFullName = "Ohio",   
                        Latitude = "39.88645",
                        FeatureCode = "PPLA2"
                    }
                }
            };
            var suggestionsParametersModel = new SuggestionsParametersModel()
            {
                Q = "Londo", 
                Latitude = "43.70011", 
                Longitude = "-79.4163"
            };
            var citiesScores = scoresCalculator.GetCitiesScores(cityModel, suggestionsParametersModel);
            var expectedResults = new List<KeyValuePair<int, decimal>>
                { new KeyValuePair<int, decimal>(cityModel.Cities[0].CityId, (decimal) 0.5) };
            
            CollectionAssert.AreEqual(expectedResults, citiesScores);    
        }
        
        [TestMethod]
        public void GetCitiesScores_GivenValuesOnlyForQParameter()
        {
            var scoresCalculator = new ScoresCalculator( new CoordinateDistanceCalculator(), new LevenshteinDistanceCalculator());
            var cityModel = new CitiesModel()
            {
                TotalResultsCount = 0,
             
                Cities = new List<City>()
                {
                    new City()
                    {
                        AdministrationCode = "OH",
                        Longitude = "-83.44825",
                        CityId = 4517009,
                        ToponymName = "London",
                        CountryId = "6252001",
                        FeatureClass = "P",
                        Population = 10060,
                        CountryCode = "US",
                        Name = "London",
                        FeatureClassName = "city, village,...",
                        AdministrationCodes = new AdministrationCodes() { ProvinceStateCode = "OH" },
                        CountryName = "United States",
                        FeatureCodeName = "seat of a second-order administrative division",
                        AdministrationFullName = "Ohio",   
                        Latitude = "39.88645",
                        FeatureCode = "PPLA2"
                    }
                }
            };
            var suggestionsParametersModel = new SuggestionsParametersModel()
            {
                Q = "Londo",
                Latitude = "0", //Longitude is set to 0 when null or white space
                Longitude = "0" //Latitude  is set to 0 when null or white space
            };
            var citiesScores = scoresCalculator.GetCitiesScores(cityModel, suggestionsParametersModel);
            var expectedResults = new List<KeyValuePair<int, decimal>>
                { new KeyValuePair<int, decimal>(cityModel.Cities[0].CityId, (decimal) 0.920044414629323) };
            
            CollectionAssert.AreEqual(expectedResults, citiesScores);    
        }
    }
}