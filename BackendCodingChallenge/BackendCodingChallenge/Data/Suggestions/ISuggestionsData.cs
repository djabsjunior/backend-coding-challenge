using System.Collections.Generic;
using BackendCodingChallenge.Models;

namespace BackendCodingChallenge.Data.Suggestions
{
    public interface ISuggestionsData
    {
        /// <summary>
        /// Returns SuggestionModel based on api/Geonames results.
        /// </summary>
        /// <param name="req"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public List<Suggestion> GetSuggestionsData(SuggestionsParametersModel parameters);
    }
}
