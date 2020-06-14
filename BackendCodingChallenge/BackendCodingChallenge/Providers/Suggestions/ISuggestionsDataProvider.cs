using System.Collections.Generic;
using BackendCodingChallenge.Models;

namespace BackendCodingChallenge.Providers.Suggestions
{
    public interface ISuggestionsDataProvider
    {
        /// <summary>
        /// Returns a list of cities suggestions based on the querystrings parameters and the Geonames API an.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<Suggestion> GetData(SuggestionsParametersModel parameters);
    }
}
