using System.Collections.Generic;

namespace BackendCodingChallenge.Models
{
    public partial class SuggestionsModel
    {
        public List<Suggestion> Suggestions { get; set; }
    }

    public partial class Suggestion
    {
        public string Name { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public double Score { get; set; }
    }
}
