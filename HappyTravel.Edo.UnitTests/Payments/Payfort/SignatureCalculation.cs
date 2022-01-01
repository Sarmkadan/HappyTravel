using System.Collections.Generic;
using HappyTravel.Edo.Api.Infrastructure.Options;
using HappyTravel.Edo.Api.Services.Payments.Payfort;
using HappyTravel.Edo.Common.Enums;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace HappyTravel.Edo.UnitTests.Payments.Payfort
{
    public class SignatureCalculation
    {
        public SignatureCalculation()
        {
            var optionsMock = new Mock<IOptions<PayfortOptions>>();
            optionsMock.Setup(o => o.Value).Returns(new PayfortOptions()
            {
                ShaResponsePhrase = "PASSRESPONSE",
                ShaRequestPhrase = "PASS",
            });
            _signatureService = new PayfortSignatureService(optionsMock.Object);
        }

        [Fact]
        public void Should_calculate_valid_request_signature()
        {
            var (_, isFailure, signature, _) = _signatureService.Calculate(_model, SignatureTypes.Request);
            Assert.False(isFailure);
            Assert.Equal("01575815bbdb23a862855bf2087012e243215e6464b6239a422d9a85936adca90bf6c3227c180890ad13b00ddf3a248608053d7174cbe622f24bdf2d5f345638",
                signature);
        }

        [Fact]
        public void Signature_field_should_not_affect_calculation()
        {
            var (_, isFailure, signature, _) = _signatureService.Calculate(_model, SignatureTypes.Request);
            _model["signature"] = "01575815bbdb23a862855bf2087012e243215e6464b6239a422d9a85936adca90bf6c3227c180890ad13b00ddf3a248608053d7174cbe622f24bdf2d5f345638";
            var (_, withSignatureFailure, withSignature, _) = _signatureService.Calculate(_model, SignatureTypes.Request);
            Assert.False(isFailure);
            Assert.False(withSignatureFailure);
            Assert.Equal(signature, withSignature);
        }

        [Fact]
        public void Should_calculate_valid_response_signature()
        {
            var (_, isFailure, signature, _) = _signatureService.Calculate(_model, SignatureTypes.Response);
            Assert.False(isFailure);
            Assert.Equal("5a79584ae629cede4f192278173b35eb3b5fd012af698ce90248e9c373047d486f5bb62394668d44edc368566a4613fa1856b4150652c649f839be33607c2831",
                signature);
        }

        [Fact]
        public void Request_and_response_signatures_should_differ()
        {
            var (_, isRequestFailure, requestSignature, _) = _signatureService.Calculate(_model, SignatureTypes.Request);
            var (_, isResponseFailure, responseSignature, _) = _signatureService.Calculate(_model, SignatureTypes.Response);
            Assert.False(isRequestFailure);
            Assert.False(isResponseFailure);
            Assert.NotEqual(requestSignature, responseSignature);
        }


        private readonly IPayfortSignatureService _signatureService;
        private const string Identifier = "MxvOupuG";
        private const string AccessCode = "SILgpo7pWbmzuURp2qri";
        private readonly Dictionary<string, string> _model = new Dictionary<string, string>()
        {
            {"command", "PURCHASE"},
            {"merchant_reference", "Test010"},
            {"amount", "1000"},
            {"access_code", AccessCode},
            {"merchant_identifier", Identifier},
            {"currency", "USD"},
            {"language", "en"},
            {"customer_email", "test@gmail.com"}
        };
    }
}