using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Payments;
using HappyTravel.Edo.Api.Services.CodeProcessors;
using HappyTravel.Edo.Api.Services.Payments.CreditCards;
using HappyTravel.Edo.Api.Services.Payments.External.PaymentLinks;
using HappyTravel.Edo.Api.Services.Payments.Payfort;
using HappyTravel.Edo.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace HappyTravel.Edo.Api.Services.Payments.External
{
    public class PaymentCallbackDispatcher : IPaymentCallbackDispatcher
    {
        public PaymentCallbackDispatcher(ICreditCardPaymentService creditCardPaymentService,
            IPaymentLinksProcessingService linksProcessingService,
            IPayfortService payfortService,
            ITagProcessor tagProcessor,
            EdoContext context)
        {
            _creditCardPaymentService = creditCardPaymentService;
            _linksProcessingService = linksProcessingService;
            _payfortService = payfortService;
            _tagProcessor = tagProcessor;
            _context = context;
        }


        public async Task<Result<PaymentResponse>> ProcessCallback(JObject response)
        {
            var (_, isFailure, paymentResponse, error) = _payfortService.ParsePaymentResponse(response);
            if (isFailure)
                return Result.Fail<PaymentResponse>(error);

            var referenceCode = paymentResponse.ReferenceCode;
            // Reference code is retrieved from 'settlement_reference' parameter in Payfort payment data object.
            if (string.IsNullOrWhiteSpace(referenceCode))
                return Result.Fail<PaymentResponse>("Settlement reference cannot be empty");

            if (!_tagProcessor.IsCodeValid(referenceCode))
                return Result.Fail<PaymentResponse>("Invalid settlement reference");

            // We have no information about where this callback from: internal (authorized customer payment) or external (payment links).
            // So we'll try to process callback sequentially with different services and return first successful result (or fail).
            var internalPaymentProcessResult = await _creditCardPaymentService.ProcessPaymentResponse(response);
            if (internalPaymentProcessResult.IsSuccess)
                return internalPaymentProcessResult;

            // Trying to process callback with payment links.
            // TODO: refactor link services to process callbacks without link code (to correspond with interface of IPaymentService.
            var linkCode = await _context.PaymentLinks
                .Where(l => l.ReferenceCode == referenceCode)
                .Select(l => l.Code)
                .SingleOrDefaultAsync();

            if (linkCode == default)
                return Result.Fail<PaymentResponse>("Invalid settlement reference");

            return await _linksProcessingService.ProcessResponse(linkCode, response);
        }


        private readonly EdoContext _context;
        private readonly ICreditCardPaymentService _creditCardPaymentService;
        private readonly IPaymentLinksProcessingService _linksProcessingService;
        private readonly IPayfortService _payfortService;

        private readonly ITagProcessor _tagProcessor;
    }
}