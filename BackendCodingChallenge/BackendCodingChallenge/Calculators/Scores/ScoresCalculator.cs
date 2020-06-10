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
            var citiesGeoCoordDistances = citiesModel.Cities.Select(city => new KeyValuePair<int, double>(city.CityId, _coordinateDistanceCalculator.ComputeDistance(double.Parse(parametersModel.Latitude), double.Parse(parametersModel.Longitude), double.Parse(city.Latitude), double.Parse(city.Longitude)))).OrderBy(c => c.Value).ToList();
            var citiesNamesDistances = citiesModel.Cities.Select(city => new KeyValuePair<int, double>(city.CityId, _levenshteinDistanceCalculator.ComputeDistance(parametersModel.Q, city.Name))).OrderBy(c => c.Value).ToList();
            var citiesScores = new List<KeyValuePair<int, double>>();

            if (string.Equals(parametersModel.Latitude, "0") && string.Equals(parametersModel.Longitude, "0"))
            {
                return ComputeScores(citiesNamesDistances, 15, 1);
            }

            var coordScores = ComputeScores(citiesGeoCoordDistances, 1000, 2);
            var namesScores = ComputeScores(citiesNamesDistances, 15, 2);

            foreach (var item in coordScores)
            {
                citiesScores.Add(new KeyValuePair<int, double>(item.Key, Math.Round(item.Value + namesScores.FirstOrDefault(n => n.Key == item.Key).Value, 1)));
            }

            return citiesScores;
        }


        /// <summary>
        /// Algorithm to calculate scores based on a  list of distances, a divisor and a base value.
        /// </summary>
        /// <param name="citiesDistances"></param>
        /// <param name="divisor"></param>
        /// <param name="algoBase"></param>
        /// <returns></returns>
        private List<KeyValuePair<int, double>> ComputeScores(List<KeyValuePair<int, double>> citiesDistances, int divisor, int algoBase)
        {
            var scores = new List<KeyValuePair<int, double>>();

            for (int i = 0; i < citiesDistances.Count; i++)
            {
                var score = Math.Exp(-(citiesDistances[i].Value / divisor)) / algoBase;
                scores.Add(new KeyValuePair<int, double>(citiesDistances[i].Key, algoBase == 1 ? Math.Round(score, 1) : score));
            }

            return scores;
        }
    }
}
