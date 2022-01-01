using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Payments;
using HappyTravel.Edo.Api.Models.Payments.External.PaymentLinks;

namespace HappyTravel.Edo.Api.Services.Payments.External.PaymentLinks
{
    public interface IPaymentLinkService
    {
        Task<Result> Send(PaymentLinkData paymentLinkData);

        Task<Result<Uri>> GenerateUri(PaymentLinkData paymentLinkData);

        ClientSettings GetClientSettings();

        List<Version> GetSupportedVersions();

        Task<Result<PaymentLinkData>> Get(string code);

        Task<Result> UpdatePaymentStatus(string code, PaymentResponse paymentResponse);
    }
}