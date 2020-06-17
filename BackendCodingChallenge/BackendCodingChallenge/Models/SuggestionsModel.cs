using System.Collections.Generic;

namespace BackendCodingChallenge.Models
{
    public class SuggestionsModel
    {
        public List<Suggestion> Suggestions { get; set; }
    }

    public class Suggestion
    {
        public string Name { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public decimal Score { get; set; }
    }
}
