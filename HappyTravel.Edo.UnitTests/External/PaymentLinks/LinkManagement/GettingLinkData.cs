using System.Linq;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Infrastructure.Converters;
using HappyTravel.Edo.Api.Infrastructure.Options;
using HappyTravel.Edo.Api.Services.CodeProcessors;
using HappyTravel.Edo.Api.Services.Payments.External.PaymentLinks;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.PaymentLinks;
using HappyTravel.Edo.UnitTests.Infrastructure.DbSetMocks;
using HappyTravel.EdoContracts.General.Enums;
using HappyTravel.MailSender;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace HappyTravel.Edo.UnitTests.External.PaymentLinks.LinkManagement
{
    public class GettingLinkData
    {
        public GettingLinkData(Mock<EdoContext> edoContextMock, IDateTimeProvider dateTimeProvider, IJsonSerializer jsonSerializer,
            ILogger<PaymentLinkService> logger)
        {
            edoContextMock.Setup(c => c.PaymentLinks)
                .Returns(DbSetMockProvider.GetDbSetMock(Links));

            var emptyOptions = Options.Create(new PaymentLinkOptions());

            _linkService = new PaymentLinkService(edoContextMock.Object,
                emptyOptions,
                Mock.Of<IMailSender>(),
                dateTimeProvider,
                jsonSerializer,
                Mock.Of<ITagProcessor>(),
                logger);
        }


        [Theory]
        [InlineData("122f")]
        [InlineData("100")]
        [InlineData("4e67ec39-8ba1-4a09-a81b-7be3191d61b8")]
        public async Task Invalid_code_should_fail(string code)
        {
            var (_, isFailure, _, _) = await _linkService.Get(code);
            Assert.True(isFailure);
        }


        [Theory]
        [InlineData(LinkCode1)]
        [InlineData(LinkCode2)]
        public async Task Valid_code_should_return_valid_link_data(string code)
        {
            var (isSuccess, _, linkData, _) = await _linkService.Get(code);
            Assert.True(isSuccess);

            var expectedLink = Links.Single(l => l.Code == code);
            AssertLinkDataIsValid();


            void AssertLinkDataIsValid()
            {
                Assert.Equal(expectedLink.Amount, linkData.Amount);
                Assert.Equal(expectedLink.Email, linkData.Email);
                Assert.Equal(expectedLink.Comment, linkData.Comment);
                Assert.Equal(expectedLink.Currency, linkData.Currency);
                Assert.Equal(expectedLink.ReferenceCode, linkData.ReferenceCode);
                Assert.Equal(expectedLink.ServiceType, linkData.ServiceType);
            }
        }
        
        
        [Fact]
        public async Task Not_existing_code_should_fail()
        {
            var (_, isFailure, _, _) = await _linkService.Get("jkpg1dbYhEe_dVwyAOgS_Q");
            Assert.True(isFailure);
        }


        private static readonly PaymentLink[] Links =
        {
            new PaymentLink
            {
                Amount = 123,
                Code = LinkCode1,
                Comment = "Comment 1",
                Currency = Currencies.AED,
                Email = "test1@email.com",
                ReferenceCode = "HTL-LNK-00012",
                ServiceType = ServiceTypes.HTL
            },
            new PaymentLink
            {
                Amount = 330,
                Code = LinkCode2,
                Comment = "Comment 2",
                Currency = Currencies.USD,
                Email = "test2@email.com",
                ReferenceCode = "HTL-LNK-00013",
                ServiceType = ServiceTypes.HTL
            }
        };

        private const string LinkCode1 = "MleKy1bt9E6QXWIVvUZqBA";
        private const string LinkCode2 = "2a4AGfe6RkWGf5eXYr8Bzg";

        private readonly PaymentLinkService _linkService;
    }
}