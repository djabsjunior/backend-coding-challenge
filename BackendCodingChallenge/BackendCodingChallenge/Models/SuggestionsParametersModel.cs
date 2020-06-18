using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BackendCodingChallenge.Models
{
    public class SuggestionsParametersModel
    {
        [BindRequired]
        public string Q { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
    }
}
