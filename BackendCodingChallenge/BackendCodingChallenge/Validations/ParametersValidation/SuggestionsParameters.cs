using BackendCodingChallenge.Models;

namespace BackendCodingChallenge.Validations.ParametersValidation
{
    public class SuggestionsParameters : ISuggestionsParameters
    {
        public SuggestionsParameters()
        {
            //Use for unit test
        }

        public bool IsValid(SuggestionsParametersModel parameters)
        {
            parameters.Latitude ??= "0";
            parameters.Longitude ??= "0";

            return !string.IsNullOrWhiteSpace(parameters.Q)
                && double.TryParse(parameters.Latitude, out _)
                && double.TryParse(parameters.Latitude, out _);
        }
    }
}
