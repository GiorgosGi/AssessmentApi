using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Provides API endpoints for performing mathematical operations
    /// </summary>
    /// <param name="service">The service used to perform mathematical calculations.</param>
    [ApiController]
    [Route("api/math")]
    public class MathController(IMathService service) : ControllerBase
    {
        private readonly IMathService _service = service ?? throw new ArgumentNullException(nameof(service));

        /// <summary>
        /// Returns the second largest distinct number from the provided array.
        /// </summary>
        /// <remarks>
        /// Example request:
        /// 
        ///     POST /second-largest
        ///     {
        ///         "requestArrayObj": [5, 20, 9, 3, 27]
        ///     }
        ///
        /// The endpoint will return the second largest **distinct** value.
        /// If no such value exists (e.g. all numbers are equal), an error is returned.
        /// </remarks>
        /// <param name="request">Object containing the array of integers.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The second largest number.</returns>
        /// <response code="200">Returns the second largest number.</response>
        /// <response code="400">Invalid input (e.g. null or too few elements).</response>
        /// <response code="500">Unexpected error.</response>
        [HttpPost("second-largest")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SecondLargestResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<SecondLargestResponseDto> GetSecondLargest([FromBody] RequestObj request, CancellationToken cancellationToken = default)
        {
            try
            {
                var value = _service.GetSecondLargest(request.RequestArrayObj, cancellationToken);
                return Ok(new SecondLargestResponseDto { Value = value });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Invalid input.", detail = ex.Message });
            }
        }
    }
}