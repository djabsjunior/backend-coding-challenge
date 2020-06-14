using BackendCodingChallenge.Models;
using BackendCodingChallenge.Providers.Suggestions;
using BackendCodingChallenge.Validations.ParametersValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendCodingChallenge.Controllers
{
    [Produces("application/json")]
    [Route("/suggestions")]
    [ApiController]
    public class SuggestionsController : Controller
    {
        private readonly ISuggestionsDataProvider _suggestionsDataProvider;

        private readonly ISuggestionsParameters _suggestionsParameters;


        public SuggestionsController(ISuggestionsDataProvider suggestionsDataProvider, ISuggestionsParameters suggestionsParameters)
        {
            _suggestionsDataProvider = suggestionsDataProvider;
            _suggestionsParameters = suggestionsParameters;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] SuggestionsParametersModel parameters)
        {
            var suggestionModel = new SuggestionsModel();

            if (!_suggestionsParameters.IsValid(parameters))
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    "Error 400: 'q' must be a string, 'longitude' and 'latitude' values must be decimals. Use a dot instead of comma for decimal's separator.");
            }

            suggestionModel.Suggestions = _suggestionsDataProvider.GetData(parameters);

            return Ok(suggestionModel);
        }
    }
}
