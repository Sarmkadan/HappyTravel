using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Models.Payments;
using HappyTravel.Edo.Api.Services.Payments;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/payments")]
    [Produces("application/json")]
    public class PaymentsHistoryController : ControllerBase
    {
        public PaymentsHistoryController(IPaymentHistoryService paymentHistoryService)
        {
            _paymentHistoryService = paymentHistoryService;
        }


        /// <summary>
        ///     Gets payment history for a current customer.
        /// </summary>
        /// <param name="companyId">The customer could have relations with different companies</param>
        /// <param name="historyRequest"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<PaymentHistoryData>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [HttpPost("history/{companyId}/customer")]
        public async Task<IActionResult> GetCustomerHistory([Required] int companyId, [FromBody] PaymentHistoryRequest historyRequest)
        {
            var (_, isFailure, response, error) = await _paymentHistoryService.GetCustomerHistory(historyRequest, companyId);
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }


        /// <summary>
        ///     Gets payment history for a company.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="historyRequest"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<PaymentHistoryData>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [HttpPost("history/{companyId}")]
        public async Task<IActionResult> GetCompanyHistory([Required] int companyId, [FromBody] PaymentHistoryRequest historyRequest)
        {
            var (_, isFailure, response, error) = await _paymentHistoryService.GetCompanyHistory(historyRequest, companyId);
            if (isFailure)
                return BadRequest(error);

            return Ok(response);
        }


        private readonly IPaymentHistoryService _paymentHistoryService;
    }
}