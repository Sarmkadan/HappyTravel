using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Accommodations;
using HappyTravel.Edo.Api.Models.Bookings;
using HappyTravel.Edo.Api.Services.Accommodations;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;
using AvailabilityRequest = HappyTravel.Edo.Api.Models.Availabilities.AvailabilityRequest;

namespace HappyTravel.Edo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}")]
    [Produces("application/json")]
    public class AccommodationsController : BaseController
    {
        public AccommodationsController(IAccommodationService service, 
            IAvailabilityService availabilityService,
            IBookingService bookingService)
        {
            _service = service;
            _availabilityService = availabilityService;
            _bookingService = bookingService;
        }


        /// <summary>
        ///     Returns the full set of accommodation details.
        /// </summary>
        /// <param name="source">Accommodation source from search results.</param>
        /// <param name="accommodationId">Accommodation ID, obtained from an availability query.</param>
        /// <returns></returns>
        [HttpGet("{source}/accommodations/{accommodationId}")]
        [ProducesResponseType(typeof(AccommodationDetails), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async ValueTask<IActionResult> Get([FromRoute] DataProviders source, [FromRoute] string accommodationId)
        {
            if (string.IsNullOrWhiteSpace(accommodationId))
                return BadRequest(ProblemDetailsBuilder.Build("No accommodation IDs was provided."));

            var (_, isFailure, response, error) = await _service.Get(source, accommodationId, LanguageCode);
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }


        /// <summary>
        ///     Returns accommodations available for a booking.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        ///     This is the "1st step" for availability search. Returns less information to choose accommodation.
        /// </remarks>
        [HttpPost("availabilities/accommodations")]
        [ProducesResponseType(typeof(CombinedAvailabilityDetails), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAvailability([FromBody] AvailabilityRequest request)
        {
            var (_, isFailure, response, error) = await _availabilityService.GetAvailable(request, LanguageCode);
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }


        /// <summary>
        ///     Returns available agreements for given accommodation and accommodation id.
        /// </summary>
        /// <param name="availabilityId">Availability id from 1-st step results.</param>
        /// <param name="source">Availability source from 1-st step results.</param>
        /// <param name="accommodationId"></param>
        /// <returns></returns>
        /// <remarks>
        ///     This is the "2nd step" for availability search. Returns richer accommodation details with agreements.
        /// </remarks>
        [HttpPost("{source}/accommodations/{accommodationId}/availabilities/{availabilityId}")]
        [ProducesResponseType(typeof(SingleAccommodationAvailabilityDetails), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAvailabilityForAccommodation([FromRoute] DataProviders source, [FromRoute] string accommodationId, [FromRoute]  long availabilityId)
        {
            var (_, isFailure, response, error) = await _availabilityService.GetAvailable(source, accommodationId, availabilityId, LanguageCode);
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }


        /// <summary>
        ///     The last 3rd search step before the booking request. Uses the exact search.
        /// </summary>
        /// <param name="source">Availability source from 1-st step results.</param>
        /// <param name="availabilityId">Availability id from the previous step</param>
        /// <param name="agreementId">Agreement id from the previous step</param>
        /// <returns></returns>
        [HttpPost("{source}/accommodations/availabilities/{availabilityId}/agreements/{agreementId}")]
        [ProducesResponseType(typeof(SingleAccommodationAvailabilityDetailsWithDeadline), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetExactAvailability([FromRoute] DataProviders source, [FromRoute] long availabilityId, [FromRoute] Guid agreementId)
        {
            var (_, isFailure, availabilityInfo, error) = await _availabilityService.GetExactAvailability(source, availabilityId, agreementId, LanguageCode);
            if (isFailure)
                return BadRequest(error);

            return Ok(availabilityInfo);
        }


        /// <summary>
        ///     Book an accommodation.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("bookings/accommodations")]
        [ProducesResponseType(typeof(BookingDetails), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Book([FromBody] AccommodationBookingRequest request)
        {
            var (_, isFailure, bookingDetails, error) = await _bookingService.Book(request, LanguageCode);
            if (isFailure)
                return BadRequest(error);

            return Ok(bookingDetails);
        }


        /// <summary>
        ///     Cancel accommodation booking.
        /// </summary>
        /// <param name="bookingId">Id of booking to cancel</param>
        /// <returns></returns>
        [HttpPost("bookings/accommodations/{bookingId}/cancel")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var (_, isFailure, error) = await _bookingService.Cancel(bookingId);
            if (isFailure)
                return BadRequest(error);

            return NoContent();
        }


        /// <summary>
        ///     Gets booking data by a booking Id.
        /// </summary>
        /// <returns>Full booking data.</returns>
        [HttpGet("bookings/accommodations/{bookingId}")]
        [ProducesResponseType(typeof(AccommodationBookingInfo), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            var (_, isFailure, bookingData, error) = await _bookingService.Get(bookingId);

            if (isFailure)
                return BadRequest(error);

            return Ok(bookingData);
        }


        /// <summary>
        ///     Gets booking data by reference code.
        /// </summary>
        /// <returns>Full booking data.</returns>
        [HttpGet("bookings/accommodations/refcode/{referenceCode}")]
        [ProducesResponseType(typeof(AccommodationBookingInfo), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBookingByReferenceCode(string referenceCode)
        {
            var (_, isFailure, bookingData, error) = await _bookingService.Get(referenceCode);

            if (isFailure)
                return BadRequest(error);

            return Ok(bookingData);
        }


        /// <summary>
        ///     Gets all bookings for a current customer.
        /// </summary>
        /// <returns>List of slim booking data.</returns>
        [ProducesResponseType(typeof(List<SlimAccommodationBookingInfo>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [HttpGet("bookings/accommodations/customer")]
        public async Task<IActionResult> GetCustomerBookings()
        {
            var (_, isFailure, bookingData, error) = await _bookingService.Get();
            if (isFailure)
                return BadRequest(error);

            return Ok(bookingData);
        }


        private readonly IAccommodationService _service;
        private readonly IAvailabilityService _availabilityService;
        private readonly IBookingService _bookingService;
    }
}