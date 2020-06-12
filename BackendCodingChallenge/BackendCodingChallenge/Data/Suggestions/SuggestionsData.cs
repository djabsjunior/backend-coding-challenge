using System;
using System.Collections.Generic;
using System.Linq;
using BackendCodingChallenge.Calculators.Scores;
using BackendCodingChallenge.Data.GeonamesAPI;
using BackendCodingChallenge.Models;

namespace BackendCodingChallenge.Data.Suggestions
{
    public class SuggestionsData : ISuggestionsData
    {
        private readonly IGeonamesApi _geonamesAPI;

        private readonly IScoresCalculator _scoresCalculator;


        public SuggestionsData(IGeonamesApi geonamesAPI, IScoresCalculator scoresCalculator)
        {
            _geonamesAPI = geonamesAPI;
            _scoresCalculator = scoresCalculator;
        }

        public SuggestionsData()
        {

        }

        public List<Suggestion> GetSuggestionsData(SuggestionsParametersModel parameters)
        {
            var suggestionList = new List<Suggestion>();

            if (string.IsNullOrWhiteSpace(parameters.Q))
            {
                return suggestionList;
            }

            var citiesData = _geonamesAPI.GetCitiesData(parameters.Q);
            var citiesScores = _scoresCalculator.GetCitiesScores(citiesData, parameters);

            foreach (var city in citiesData.Cities.Where(c => citiesScores.Exists(cs => cs.Key == c.CityId)))
            {
                string[] cityNameArray = { city.Name, city.AdministrationCodes.ProvinceStateCode, city.CountryCode };

                suggestionList.Add(new Suggestion
                {
                    Latitude = city.Latitude,
                    Longitude = city.Longitude,
                    Name = string.Join(", ", cityNameArray),
                    Score = citiesScores.FirstOrDefault(cs => cs.Key == city.CityId).Value
                });
            }

            return suggestionList.OrderByDescending(s => s.Score).ToList();
        }
    }
}
