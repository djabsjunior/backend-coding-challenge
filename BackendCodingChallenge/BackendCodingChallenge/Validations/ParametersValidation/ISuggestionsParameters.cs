using BackendCodingChallenge.Models;

namespace BackendCodingChallenge.Validations.ParametersValidation
{
    public interface ISuggestionsParameters
    {
        /// <summary>
        /// Validate query parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool IsValid(SuggestionsParametersModel parameters);
    }
}
