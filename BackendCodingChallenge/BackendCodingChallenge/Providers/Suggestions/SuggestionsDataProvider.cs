using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BackendCodingChallenge.Calculators.Scores;
using BackendCodingChallenge.Data.GeonamesAPI;
using BackendCodingChallenge.Models;

namespace BackendCodingChallenge.Providers.Suggestions
{
    public class SuggestionsDataProvider : ISuggestionsDataProvider
    {
        private readonly IGeonamesApi _geonamesApi;

        private readonly IScoresCalculator _scoresCalculator;


        public SuggestionsDataProvider(IGeonamesApi geonamesApi, IScoresCalculator scoresCalculator)
        {
            _geonamesApi = geonamesApi;
            _scoresCalculator = scoresCalculator;
        }

        public SuggestionsDataProvider()
        {
            //for unit tests
        }

        public List<Suggestion> GetData(SuggestionsParametersModel parameters)
        {
            var suggestionList = new List<Suggestion>();

            if (string.IsNullOrWhiteSpace(parameters.Q))
            {
                return suggestionList;
            }

            var citiesData = _geonamesApi.GetCitiesData(parameters.Q);
            var citiesScores = _scoresCalculator.GetCitiesScores(citiesData, parameters);

            foreach (var city in citiesData.Cities)
            {
                string[] cityNameArray = { city.Name, city.AdministrationCodes.ProvinceStateCode, city.CountryCode };
                var cityScore = Convert.ToDecimal(Math.Round(citiesScores.FirstOrDefault(cs => cs.Key == city.CityId).Value, 1)
                    .ToString("0.#"));
                
                suggestionList.Add(new Suggestion
                {
                    Latitude = city.Latitude,
                    Longitude = city.Longitude,
                    Name = string.Join(", ", cityNameArray),
                    Score = cityScore
                });
            }

            return suggestionList.OrderByDescending(s => s.Score).ToList();
        }
    }
}
