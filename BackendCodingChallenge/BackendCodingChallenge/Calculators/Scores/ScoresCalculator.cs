using System;
using System.Collections.Generic;
using System.Linq;
using BackendCodingChallenge.Calculators.CoordinateDistance;
using BackendCodingChallenge.Calculators.LevenshteinDistance;
using BackendCodingChallenge.Models;

namespace BackendCodingChallenge.Calculators.Scores
{
    public class ScoresCalculator : IScoresCalculator
    {
        private readonly ICoordinateDistanceCalculator _coordinateDistanceCalculator;

        private readonly ILevenshteinDistanceCalculator _levenshteinDistanceCalculator;


        public ScoresCalculator(ICoordinateDistanceCalculator coordinateDistanceCalculator, ILevenshteinDistanceCalculator levenshteinDistanceCalculator)
        {
            _coordinateDistanceCalculator = coordinateDistanceCalculator;
            _levenshteinDistanceCalculator = levenshteinDistanceCalculator;
        }

        public List<KeyValuePair<int, decimal>> GetCitiesScores(CitiesModel citiesModel, SuggestionsParametersModel parametersModel)
        {
            var citiesGeoCoordDistances = citiesModel.Cities.Select(city => new KeyValuePair<int, decimal>(city.CityId,
                _coordinateDistanceCalculator.ComputeDistance(decimal.Parse(parametersModel.Latitude),
                    decimal.Parse(parametersModel.Longitude), decimal.Parse(city.Latitude),
                    decimal.Parse(city.Longitude)))).OrderBy(c => c.Value)
                .ToList();

            var citiesNamesDistances = citiesModel.Cities
                .Select(city => new KeyValuePair<int, decimal>(city.CityId,
                    _levenshteinDistanceCalculator.ComputeDistance(parametersModel.Q, city.Name))).OrderBy(c => c.Value)
                .ToList();

            if (string.Equals(parametersModel.Latitude, "0") && string.Equals(parametersModel.Longitude, "0"))
            {
                return ComputeNamesScores(citiesNamesDistances, false);
            }

            var coordScores = ComputeGeoCoordScores(citiesGeoCoordDistances);
            var namesScores = ComputeNamesScores(citiesNamesDistances, true);

            return coordScores.Select(item => new KeyValuePair<int, decimal>(item.Key,
                item.Value + namesScores.FirstOrDefault(n => n.Key == item.Key).Value))
                .ToList();
        }


        /// <summary>
        /// Algorithm to calculate scores based on geocoordinates' distances.
        /// </summary>
        /// <param name="citiesDistances"></param>
        /// <returns></returns>
        private List<KeyValuePair<int, decimal>> ComputeGeoCoordScores(List<KeyValuePair<int, decimal>> citiesDistances)
        {
            var scores = new List<KeyValuePair<int, decimal>>();

            foreach (var (cityId, distance) in citiesDistances)
            {
                decimal score = 0;
                
                if (distance <= 500)
                {
                    score = Convert.ToDecimal(Math.Exp(decimal.ToDouble(-(distance / 1000))) / 2);
                }
                
                scores.Add(new KeyValuePair<int, decimal>(cityId, score));
            }

            return scores;
        }
        
        /// <summary>
        /// Algorithm to calculate scores according to names' distances and a param set to true when geo coordinates are involved
        /// and false otherwise).
        /// </summary>
        /// <param name="citiesDistances"></param>
        /// <param name="geoCoordInvolved"></param>
        /// <returns></returns>
        private List<KeyValuePair<int, decimal>> ComputeNamesScores(List<KeyValuePair<int, decimal>> citiesDistances, bool geoCoordInvolved)
        {
            var scores = new List<KeyValuePair<int, decimal>>();

            foreach (var (cityId, distance) in citiesDistances)
            {
                var score = 0.5;
                var d = decimal.ToDouble(distance);
                
                score = distance <= 4 ? 
                    geoCoordInvolved 
                        ? score 
                        : Math.Exp(-(d / 12)) 
                    : geoCoordInvolved 
                        ? score - Math.Exp(-(d / 4))
                        : score;

                scores.Add(new KeyValuePair<int, decimal>(cityId, Convert.ToDecimal(score)));
            }

            return scores;
        }
    }
}
