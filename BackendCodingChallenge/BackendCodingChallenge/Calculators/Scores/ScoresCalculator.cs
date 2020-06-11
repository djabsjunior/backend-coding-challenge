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

        public List<KeyValuePair<int, double>> GetCitiesScores(CitiesModel citiesModel, SuggestionsParametersModel parametersModel)
        {
            var citiesGeoCoordDistances = citiesModel.Cities.Select(city => new KeyValuePair<int, double>(city.CityId,
                _coordinateDistanceCalculator.ComputeDistance(double.Parse(parametersModel.Latitude),
                    double.Parse(parametersModel.Longitude), double.Parse(city.Latitude),
                    double.Parse(city.Longitude)))).OrderBy(c => c.Value)
                .ToList();

            var citiesNamesDistances = citiesModel.Cities
                .Select(city => new KeyValuePair<int, double>(city.CityId,
                    _levenshteinDistanceCalculator.ComputeDistance(parametersModel.Q, city.Name))).OrderBy(c => c.Value)
                .ToList();

            if (string.Equals(parametersModel.Latitude, "0") && string.Equals(parametersModel.Longitude, "0"))
            {
                return ComputeNamesScores(citiesNamesDistances, false);
            }

            var coordScores = ComputeGeoCoordScores(citiesGeoCoordDistances);
            var namesScores = ComputeNamesScores(citiesNamesDistances, true);

            return coordScores.Select(item => new KeyValuePair<int, double>(item.Key,
                Math.Round(item.Value + namesScores.FirstOrDefault(n => n.Key == item.Key).Value, 1))).ToList();
        }


        /// <summary>
        /// Algorithm to calculate scores based on geocoordinates' distances.
        /// </summary>
        /// <param name="citiesDistances"></param>
        /// <returns></returns>
        private List<KeyValuePair<int, double>> ComputeGeoCoordScores(List<KeyValuePair<int, double>> citiesDistances)
        {
            var scores = new List<KeyValuePair<int, double>>();

            foreach (var (cityId, distance) in citiesDistances)
            {
                double score = 0;
                
                if (distance <= 500)
                {
                    score = Math.Exp(-(distance / 1000)) / 2;
                }
                
                scores.Add(new KeyValuePair<int, double>(cityId, score));
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
        private List<KeyValuePair<int, double>> ComputeNamesScores(List<KeyValuePair<int, double>> citiesDistances, bool geoCoordInvolved)
        {
            var scores = new List<KeyValuePair<int, double>>();

            foreach (var (cityId, distance) in citiesDistances)
            {
                var score = 0.5;
                
                score = distance <= 4 ? 
                    geoCoordInvolved 
                        ? score 
                        : Math.Exp(-(distance / 12)) 
                    : geoCoordInvolved 
                        ? 0.5 - Math.Exp(-(distance / 4))
                        : score;

                scores.Add(new KeyValuePair<int, double>(cityId, !geoCoordInvolved ? Math.Round(score, 1) : score));
            }

            return scores;
        }
    }
}
