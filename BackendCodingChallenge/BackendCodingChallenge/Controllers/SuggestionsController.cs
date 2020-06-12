using BackendCodingChallenge.Data.Suggestions;
using BackendCodingChallenge.Models;
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
        private readonly ISuggestionsData _suggestionsData;

        private readonly ISuggestionsParameters _suggestionsParameters;


        public SuggestionsController(ISuggestionsData suggestionsData, ISuggestionsParameters suggestionsParameters)
        {
            _suggestionsData = suggestionsData;
            _suggestionsParameters = suggestionsParameters;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] SuggestionsParametersModel parameters)
        {
            var suggestionModel = new SuggestionsModel();

            if (!_suggestionsParameters.IsValid(parameters))
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    "Error 400: 'q' must be a string, 'longitude' and 'latitude' values must be decimal. Use a dot instead of comma for decimal's separator.");
            }

            suggestionModel.Suggestions = _suggestionsData.GetSuggestionsData(parameters);

            return Ok(suggestionModel);
        }
    }
}
