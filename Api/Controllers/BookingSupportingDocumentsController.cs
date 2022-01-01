using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Mailing;
using HappyTravel.Edo.Api.Services.Accommodations;
using HappyTravel.Edo.Api.Services.Mailing;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/accommodations/supporting-documentation")]
    [Produces("application/json")]
    public class BookingSupportingDocumentsController : ControllerBase
    {
        public BookingSupportingDocumentsController(IBookingMailingService bookingMailingService,
            IBookingDocumentsService bookingDocumentsService)
        {
            _bookingMailingService = bookingMailingService;
            _bookingDocumentsService = bookingDocumentsService;
        }


        /// <summary>
        ///     Sends booking voucher to an email.
        /// </summary>
        /// <param name="bookingId">Id of the booking.</param>
        /// <param name="email">E-mail to send voucher.</param>
        /// <returns></returns>
        [HttpPost("{bookingId}/voucher/send")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendBookingVoucher([Required] int bookingId, [Required] string email)
        {
            var (_, isFailure, error) = await _bookingMailingService.SendVoucher(bookingId, email);

            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok("Booking voucher has been sent");
        }


        /// <summary>
        ///     Sends booking invoice to an email.
        /// </summary>
        /// <param name="bookingId">Id of the booking.</param>
        /// <param name="email">E-mail to send voucher.</param>
        /// <returns></returns>
        [HttpPost("{bookingId}/invoice/send")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendBookingInvoice([Required] int bookingId, [Required] string email)
        {
            var (_, isFailure, error) = await _bookingMailingService.SendInvoice(bookingId, email);

            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok("Booking invoice has been sent");
        }


        /// <summary>
        ///     Gets booking voucher data.
        /// </summary>
        /// <param name="bookingId">Id of the booking.</param>
        /// <returns>Voucher data,</returns>
        [HttpGet("{bookingId}/voucher")]
        [ProducesResponseType(typeof(BookingVoucherData), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBookingVoucher([Required] int bookingId)
        {
            var (_, isFailure, voucher, error) = await _bookingDocumentsService.GenerateVoucher(bookingId);

            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(voucher);
        }


        /// <summary>
        ///     Gets booking invoice.
        /// </summary>
        /// <param name="bookingId">Id of the booking.</param>
        /// <returns>Invoice data.</returns>
        [HttpGet("{bookingId}/invoice")]
        [ProducesResponseType(typeof(BookingInvoiceData), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetBookingInvoice([Required] int bookingId)
        {
            var (_, isFailure, invoice, error) = await _bookingDocumentsService.GenerateInvoice(bookingId);

            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(invoice);
        }


        private readonly IBookingDocumentsService _bookingDocumentsService;

        private readonly IBookingMailingService _bookingMailingService;
    }
}