using System.Net;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Infrastructure.Http;
using HappyTravel.Edo.Api.Services.ProviderResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}")]
    [Produces("application/json")]
    public class BookingResponseController : BaseController
    {
        public BookingResponseController(INetstormingResponseService netstormingResponseService)
        {
            _netstormingResponseService = netstormingResponseService;
        }
        
        
        /// <summary>
        /// Netstorming sends XML responses with booking details on this route.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [HttpPost("bookings/accommodations/responses/netstorming")]
        public async Task<IActionResult> HandleNetstormingBookingResponse()
        {
            var (_, isXmlRequestFailure, xmlRequestData, xmlRequestError) = await RequestHelper.GetAsBytes(HttpContext.Request.Body);
            if (isXmlRequestFailure)
                return BadRequest(new ProblemDetails
                {
                    Detail = xmlRequestError,
                    Status = (int) HttpStatusCode.BadRequest
                });
            
            var (_, isFailure, error) = await _netstormingResponseService.ProcessBookingDetailsResponse(xmlRequestData);
            if (isFailure)
                return BadRequest(error);
            
            return Ok();
        }


        private readonly INetstormingResponseService _netstormingResponseService;
    }
}