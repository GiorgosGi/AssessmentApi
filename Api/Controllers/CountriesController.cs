using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Provides operations for retrieving country information.
    /// </summary>
    [ApiController]
    [Route("api/countries")]
    public class CountriesController(ICountryService service, ILogger<CountriesController> logger) : ControllerBase
    {
        private readonly ICountryService _service = service ?? throw new ArgumentNullException(nameof(service));
        private readonly ILogger<CountriesController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Retrieves the list of all available countries.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a collection of countries with their basic details:
        /// The common name of the country
        /// The capital of the country
        /// The borders of the country
        /// 
        /// Possible scenarios:
        /// - Success: Returns a list of countries.
        /// - Service unavailable: External dependency or service failure.
        /// - Unexpected error: Any unhandled exception.
        /// </remarks>
        /// <returns>A list of countries.</returns>
        /// <response code="200">Successfully retrieved the list of countries.</response>
        /// <response code="503">Service temporarily unavailable.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CountryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CountryDto>>> Get(CancellationToken cancellationToken = default)
        {
            try
            {
                var countries = await _service.GetCountriesAsync(cancellationToken);
                return Ok(countries);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Service is currently unavailable.");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, 
                    new { error = "Unable to retrieve countries at this time." });
            }
        }
    }
}