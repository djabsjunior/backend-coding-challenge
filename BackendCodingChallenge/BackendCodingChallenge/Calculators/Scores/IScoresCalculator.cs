using System.Collections.Generic;
using BackendCodingChallenge.Models;

namespace BackendCodingChallenge.Calculators.Scores
{
    public interface IScoresCalculator
    {
        /// <summary>
        /// Returns the score between 0 and 0.5 using the BBF algorithm
        /// Ref: https://www.cs.ubc.ca/~lowe/papers/ijcv04.pdf
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, double>> GetCitiesScores(CitiesModel citiesModel, SuggestionsParametersModel parametersModel);


    }
}
