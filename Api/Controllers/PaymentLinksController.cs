using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Payments;
using HappyTravel.Edo.Api.Models.Payments.External.PaymentLinks;
using HappyTravel.Edo.Api.Services.Payments;
using HappyTravel.Edo.Api.Services.Payments.CreditCards;
using HappyTravel.Edo.Api.Services.Payments.External.PaymentLinks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace HappyTravel.Edo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/external/payment-links")]
    [Produces("application/json")]
    public class PaymentLinksController : BaseController
    {
        public PaymentLinksController(IPaymentLinkService paymentLinkService,
            IPaymentLinksProcessingService paymentLinksProcessingService,
            ICreditCardService cardService)
        {
            _paymentLinkService = paymentLinkService;
            _paymentLinksProcessingService = paymentLinksProcessingService;
            _cardService = cardService;
        }


        /// <summary>
        ///     Gets supported desktop client versions.
        /// </summary>
        /// <returns>List of supported versions.</returns>
        [HttpGet("versions")]
        [ProducesResponseType(typeof(List<Version>), (int) HttpStatusCode.OK)]
        public IActionResult GetSupportedDesktopAppVersion() => Ok(_paymentLinkService.GetSupportedVersions());


        /// <summary>
        ///     Gets client settings for payment links.
        /// </summary>
        /// <returns>Payment link settings.</returns>
        [HttpGet("settings")]
        [ProducesResponseType(typeof(ClientSettings), (int) HttpStatusCode.OK)]
        public IActionResult GetSettings() => Ok(_paymentLinkService.GetClientSettings());


        /// <summary>
        ///     Sends payment link to specified e-mail address.
        /// </summary>
        /// <param name="request">Payment link data</param>
        /// <returns></returns>
        [HttpPost("send")]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> SendLink([FromBody] PaymentLinkData request)
        {
            var (isSuccess, _, error) = await _paymentLinkService.Send(request);
            return isSuccess
                ? NoContent()
                : (IActionResult) BadRequest(ProblemDetailsBuilder.Build(error));
        }


        /// <summary>
        ///     Generates payment link.
        /// </summary>
        /// <param name="request">Payment link data</param>
        /// <returns>Payment link data.</returns>
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateUrl([FromBody] PaymentLinkData request)
        {
            var (isSuccess, _, uri, error) = await _paymentLinkService.GenerateUri(request);
            return isSuccess
                ? Ok(uri)
                : (IActionResult) BadRequest(ProblemDetailsBuilder.Build(error));
        }


        /// <summary>
        ///     Gets payment link by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Payment link data.</returns>
        [HttpGet("{code}")]
        [AllowAnonymous]
        [RequestSizeLimit(256)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(PaymentLinkData), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetPaymentData([Required] string code)
        {
            var (isSuccess, _, linkData, error) = await _paymentLinkService.Get(code);
            return isSuccess
                ? Ok(linkData)
                : (IActionResult) BadRequest(ProblemDetailsBuilder.Build(error));
        }


        /// <summary>
        ///     Calculates signature for link with specified code.
        /// </summary>
        /// <param name="code">Payment link code.</param>
        /// <param name="request">JSON request.</param>
        /// <returns>Signature.</returns>
        [AllowAnonymous]
        [RequestSizeLimit(512)]
        [HttpPost("{code}/sign")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CalculateSignature(string code, [FromBody] JObject request)
        {
            // TODO: Change JObject to strict model.
            var customProperties = GetReferenceAndFingerprint(request);
            if (customProperties.IsFailure)
                return BadRequest(ProblemDetailsBuilder.Build(customProperties.Error));

            var deviceFingerprint = customProperties.Value.DeviceFingerprint;
            var merchantReference = customProperties.Value.MerchantReference;

            var (_, isFailure, signature, error) = await _paymentLinksProcessingService.CalculateSignature(code,
                merchantReference,
                deviceFingerprint,
                LanguageCode);

            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(signature);


            Result<(string MerchantReference, string DeviceFingerprint)> GetReferenceAndFingerprint(JObject jObject)
            {
                var propertiesDictionary = request.Properties()
                    .ToDictionary(p => p.Name, p => p.Value.Value<object>()?.ToString());

                const string merchantReferenceKey = "merchant_reference";
                var isGetMerchantReferenceSuccess = propertiesDictionary
                    .TryGetValue(merchantReferenceKey, out var reference);

                if (!isGetMerchantReferenceSuccess)
                    return Result.Fail<(string, string)>($"'{merchantReferenceKey}' value is required");

                const string deviceFingerprintKey = "device_fingerprint";

                // Fingerprint can be null.
                propertiesDictionary
                    .TryGetValue(deviceFingerprintKey, out var fingerprint);

                return Result.Ok((reference, fingerprint));
            }
        }


        /// <summary>
        ///     Executes payment for link.
        /// </summary>
        /// <param name="code">Payment link code.</param>
        /// <param name="token">Payment token.</param>
        /// <returns>Payment result. Can return data for further 3DSecure processing.</returns>
        [HttpPost("{code}/pay")]
        [AllowAnonymous]
        [RequestSizeLimit(512)]
        [ProducesResponseType(typeof(PaymentResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Pay([Required] string code, [FromBody] [Required] string token)
        {
            var (isSuccess, _, paymentResponse, error) = await _paymentLinksProcessingService.Pay(code,
                token,
                ClientIp,
                LanguageCode);

            return isSuccess
                ? Ok(paymentResponse)
                : (IActionResult) BadRequest(ProblemDetailsBuilder.Build(error));
        }


        /// <summary>
        ///     Processed payment callback. Typically is used with 3D Secure payment flow.
        /// </summary>
        /// <param name="code">Payment link code.</param>
        /// <param name="value">Payment data, returned from payment system, in JSON format.</param>
        /// <returns>Payment result.</returns>
        [HttpPost("{code}/pay/callback")]
        [AllowAnonymous]
        [RequestSizeLimit(1024)]
        [ProducesResponseType(typeof(PaymentResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PaymentCallback([Required] string code, [FromBody] JObject value)
        {
            var (isSuccess, _, paymentResponse, error) = await _paymentLinksProcessingService.ProcessResponse(code, value);
            return isSuccess
                ? Ok(paymentResponse)
                : (IActionResult) BadRequest(ProblemDetailsBuilder.Build(error));
        }


        /// <summary>
        ///     Gets settings for tokenization
        /// </summary>
        /// <returns>Settings for tokenization</returns>
        [HttpGet("tokenization-settings")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenizationSettings), (int) HttpStatusCode.OK)]
        public IActionResult GetTokenizationSettings() => Ok(_cardService.GetTokenizationSettings());


        private readonly ICreditCardService _cardService;


        private readonly IPaymentLinkService _paymentLinkService;
        private readonly IPaymentLinksProcessingService _paymentLinksProcessingService;
    }
}