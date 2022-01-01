using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Locations;
using HappyTravel.Edo.Api.Services.Customers;
using HappyTravel.Edo.Api.Services.Locations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/locations")]
    [Produces("application/json")]
    public class LocationsController : BaseController
    {
        public LocationsController(ICustomerContext customerContext, ILocationService service)
        {
            _customerContext = customerContext;
            _service = service;
        }


        /// <summary>
        ///     Returns a list of world countries.
        /// </summary>
        /// <param name="query">The search query text.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("countries")]
        [ProducesResponseType(typeof(List<Country>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetCountries([FromQuery] string query) => Ok(await _service.GetCountries(query, LanguageCode));


        /// <summary>
        ///     Returns location predictions what a used when searching
        /// </summary>
        /// <param name="query">The search query text.</param>
        /// <param name="sessionId">The search session ID.</param>
        /// <returns></returns>
        [HttpGet("predictions")]
        [ProducesResponseType(typeof(List<Prediction>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLocationPredictions([FromQuery] string query, [FromQuery] [Required] string sessionId)
        {
            var (_, isCustomerFailure, customerInfo, customerError) = await _customerContext.GetCustomerInfo();
            if (isCustomerFailure)
                return BadRequest(ProblemDetailsBuilder.Build(customerError));

            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(ProblemDetailsBuilder.Build($"'{nameof(query)}' is required."));

            //TODO: remove customer ID check when locality restriction will be removed (NIJO-345)
            var (_, isFailure, value, error) = await _service.GetPredictions(query, sessionId, customerInfo.CustomerId, LanguageCode);
            return isFailure
                ? (IActionResult) BadRequest(error)
                : Ok(value);
        }


        /// <summary>
        ///     Returns a list of world regions.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("regions")]
        [ProducesResponseType(typeof(List<Region>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetRegions() => Ok(await _service.GetRegions(LanguageCode));


        /// <summary>
        ///     Internal. Sets locations, gathered from booking sources, to make predictions.
        /// </summary>
        /// <param name="locations"></param>
        /// <returns></returns>
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [HttpPost]
        public async ValueTask<IActionResult> SetPredictions([FromBody] IEnumerable<Location> locations)
        {
            if (locations is null || !locations.Any())
                return NoContent();

            await _service.Set(locations);
            return NoContent();
        }


        /// <summary>
        ///     Internal. Gets date of last modified location. This can be treated as last locations update date.
        /// </summary>
        /// <returns>Last changed location modified date</returns>
        [ProducesResponseType(typeof(DateTime), (int) HttpStatusCode.OK)]
        [HttpGet("last-modified-date")]
        public async Task<IActionResult> GetLastModifiedDate()
        {
            var lastModifiedDate = await _service.GetLastModifiedDate();

            return Ok(lastModifiedDate);
        }


        private readonly ICustomerContext _customerContext;
        private readonly ILocationService _service;
    }
}