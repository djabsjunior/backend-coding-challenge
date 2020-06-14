using System.Collections.Generic;
using BackendCodingChallenge.Models;

namespace BackendCodingChallenge.Calculators.Scores
{
    public interface IScoresCalculator
    {
        /// <summary>
        /// Returns the score between 0 and 1 using a custom algorithm
        /// </summary>
        /// <param name="citiesModel"></param>
        /// <param name="parametersModel"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, decimal>> GetCitiesScores(CitiesModel citiesModel, SuggestionsParametersModel parametersModel);


    }
}
