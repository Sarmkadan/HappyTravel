using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Emailing;
using HappyTravel.Edo.Api.Services.Mailing;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/bookings")]
    [Produces("application/json")]
    public class BookingEmailingController : ControllerBase
    {
        public BookingEmailingController(IBookingMailingService bookingMailingService)
        {
            _bookingMailingService = bookingMailingService;
        }


        /// <summary>
        ///     Sends booking voucher to an email.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="bookingDocumentRequest"></param>
        /// <returns></returns>
        [Obsolete("Use methods from booking documents controller")]
        [HttpPost("accommodations/{bookingId}/voucher")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendBookingVoucher([Required] int bookingId, [Required] [FromBody] SendBookingDocumentRequest bookingDocumentRequest)
        {
            var (_, isFailure, error) = await _bookingMailingService.SendVoucher(bookingId, bookingDocumentRequest.Email);

            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok("Booking voucher has been sent");
        }


        /// <summary>
        ///     Sends booking invoice to an email.
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="bookingDocumentRequest"></param>
        /// <returns></returns>
        [Obsolete("Use methods from booking documents controller")]
        [HttpPost("accommodations/{bookingId}/invoice")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendBookingInvoice([Required] int bookingId, [Required] [FromBody] SendBookingDocumentRequest bookingDocumentRequest)
        {
            var (_, isFailure, error) = await _bookingMailingService.SendInvoice(bookingId, bookingDocumentRequest.Email);

            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok("Booking invoice has been sent");
        }


        private readonly IBookingMailingService _bookingMailingService;
    }
}