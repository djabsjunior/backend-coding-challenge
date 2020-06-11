using System.Collections.Generic;
using BackendCodingChallenge.Data.Suggestions;
using BackendCodingChallenge.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendCodingChallenge.UnitTest.DataTests
{
    [TestClass]    
    public class SuggestionsDataTests
    {
        [TestMethod]
        public void GetSuggestionsData_NullParameters_ReturnEmptyListOfSuggestions()
        {
            var suggestionsData = new SuggestionsData();
            var suggestionDataList = suggestionsData.GetSuggestionsData(new SuggestionsParametersModel());
            
            CollectionAssert.AreEqual(new List<Suggestion>(), suggestionDataList);
        }
    }
}