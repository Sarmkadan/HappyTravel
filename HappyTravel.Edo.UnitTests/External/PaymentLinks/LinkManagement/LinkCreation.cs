using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Infrastructure.Converters;
using HappyTravel.Edo.Api.Infrastructure.Options;
using HappyTravel.Edo.Api.Models.Payments.External.PaymentLinks;
using HappyTravel.Edo.Api.Services.CodeProcessors;
using HappyTravel.Edo.Api.Services.Payments.External.PaymentLinks;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.PaymentLinks;
using HappyTravel.EdoContracts.General.Enums;
using HappyTravel.MailSender;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace HappyTravel.Edo.UnitTests.External.PaymentLinks.LinkManagement
{
    public class LinkCreation
    {
        public LinkCreation(Mock<EdoContext> edoContextMock,
            ILogger<PaymentLinkService> logger,
            IJsonSerializer jsonSerializer, IDateTimeProvider dateTimeProvider)
        {
            _edoContextMock = edoContextMock;
            _logger = logger;
            _jsonSerializer = jsonSerializer;
            _dateTimeProvider = dateTimeProvider;

            _edoContextMock
                .Setup(e => e.PaymentLinks.Add(It.IsAny<PaymentLink>()))
                .Callback<PaymentLink>(link => LastCreatedLink = link);
        }


        [Theory]
        [MemberData(nameof(ValidLinkDataList))]
        public async Task Generate_link_should_store_link_in_db(PaymentLinkData paymentLinkData)
        {
            var linkService = CreateService();
            var (_, isFailure, _, _) = await linkService.GenerateUri(paymentLinkData);

            Assert.False(isFailure);
            AssertLinkDataIsStored(paymentLinkData);
        }


        [Theory]
        [MemberData(nameof(ValidLinkDataList))]
        public async Task Send_link_should_store_link_in_db(PaymentLinkData paymentLinkData)
        {
            var linkService = CreateService(mailSender: GetMailSenderWithOkResult());
            var (_, isFailure, _) = await linkService.Send(paymentLinkData);

            Assert.False(isFailure);
            AssertLinkDataIsStored(paymentLinkData);


            IMailSender GetMailSenderWithOkResult()
            {
                var mailSenderMock = new Mock<IMailSender>();
                mailSenderMock.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                    .ReturnsAsync(Result.Ok);

                return mailSenderMock.Object;
            }
        }


        [Theory]
        [MemberData(nameof(ValidLinkDataList))]
        public async Task Send_link_should_send_link_by_email(PaymentLinkData paymentLinkData)
        {
            var mailSenderMock = CreateMailSenderMockWithCallback();
            var linkService = CreateService(mailSender: mailSenderMock.Object);
            var (_, isFailure, _) = await linkService.Send(paymentLinkData);

            Assert.False(isFailure);
            Assert.Equal(LastSentMailData.addressee, paymentLinkData.Email);


            Mock<IMailSender> CreateMailSenderMockWithCallback()
            {
                var senderMock = new Mock<IMailSender>();
                senderMock.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                    .ReturnsAsync(Result.Ok)
                    .Callback<string, string, object>((templateId, addressee, mailData) => LastSentMailData = (templateId, addressee, mailData));

                return senderMock;
            }
        }


        [Theory]
        [MemberData(nameof(LinkDataConflictingWithSettings))]
        public async Task Creating_link_should_validate_against_client_settings(PaymentLinkData paymentLinkData)
        {
            var linkService = CreateService(GetOptions());

            var (_, isGenerateFailure, _, _) = await linkService.GenerateUri(paymentLinkData);
            Assert.True(isGenerateFailure);

            var (_, isSendFailure, _) = await linkService.Send(paymentLinkData);
            Assert.True(isSendFailure);


            IOptions<PaymentLinkOptions> GetOptions()
                => Options.Create(new PaymentLinkOptions
                {
                    ClientSettings = new ClientSettings
                    {
                        Currencies = new List<Currencies> {Currencies.AED, Currencies.EUR},
                        ServiceTypes = new Dictionary<ServiceTypes, string>
                        {
                            {ServiceTypes.HTL, "Hotel booking"}
                        }
                    }
                });
        }


        [Theory]
        [MemberData(nameof(InvalidLinkDataList))]
        public async Task Create_link_should_fail_for_invalid_data(PaymentLinkData paymentLinkData)
        {
            var linkService = CreateService();
            var (_, isSendFailure, _) = await linkService.Send(paymentLinkData);
            Assert.True(isSendFailure);

            var (_, isGenerateFailure, _) = await linkService.GenerateUri(paymentLinkData);
            Assert.True(isGenerateFailure);
        }
        
        [Fact]
        public async Task Failed_to_send_link_should_fail()
        {
            var linkService = CreateService(mailSender: GetMailSenderWithFailResult());
            var paymentLinkData = (PaymentLinkData) ValidLinkDataList[0][0];

            LastCreatedLink = null;
            var (_, isFailure, _) = await linkService.Send(paymentLinkData);

            Assert.True(isFailure);


            IMailSender GetMailSenderWithFailResult()
            {
                var mailSenderMock = new Mock<IMailSender>();
                mailSenderMock.Setup(m => m.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                    .ReturnsAsync(Result.Fail("Some error"));

                return mailSenderMock.Object;
            }
        }


        private PaymentLinkService CreateService(IOptions<PaymentLinkOptions> options = null,
            IMailSender mailSender = null,
            ITagProcessor tagProcessor = null)
        {
            options = options ?? GetValidOptions();
            mailSender = mailSender ?? Mock.Of<IMailSender>();
            tagProcessor = tagProcessor ?? Mock.Of<ITagProcessor>();

            return new PaymentLinkService(_edoContextMock.Object,
                options,
                mailSender,
                _dateTimeProvider,
                _jsonSerializer,
                tagProcessor,
                _logger);


            IOptions<PaymentLinkOptions> GetValidOptions()
                => Options.Create(new PaymentLinkOptions
                {
                    ClientSettings = new ClientSettings
                    {
                        Currencies = new List<Currencies> {Currencies.AED, Currencies.EUR},
                        ServiceTypes = new Dictionary<ServiceTypes, string>
                        {
                            {ServiceTypes.HTL, "Hotel booking"},
                            {ServiceTypes.TRN, "Airport transfer"}
                        }
                    },
                    SupportedVersions = new List<Version> {new Version(0, 2)},
                    MailTemplateId = "templateId_fkIfu423_-e",
                    PaymentUrlPrefix = new Uri("https://test/prefix")
                });
        }


        private void AssertLinkDataIsStored(PaymentLinkData paymentLinkData)
        {
            Assert.Equal(paymentLinkData.Amount, LastCreatedLink.Amount);
            Assert.Equal(paymentLinkData.Comment, LastCreatedLink.Comment);
            Assert.Equal(paymentLinkData.Currency, LastCreatedLink.Currency);
            Assert.Equal(paymentLinkData.Email, LastCreatedLink.Email);
            Assert.Equal(paymentLinkData.ServiceType, LastCreatedLink.ServiceType);
        }


        private readonly Mock<EdoContext> _edoContextMock;
        private readonly ILogger<PaymentLinkService> _logger;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IDateTimeProvider _dateTimeProvider;
        private (string templateId, string addressee, object linkData) LastSentMailData { get; set; }
        private PaymentLink LastCreatedLink { get; set; }

        public static readonly object[][] ValidLinkDataList =
        {
            new object[] {new PaymentLinkData(121, "hit@yy.com", ServiceTypes.HTL, Currencies.EUR, "comment1", default, default)},
            new object[] {new PaymentLinkData((decimal) 433.1, "antuan@xor.com", ServiceTypes.TRN, Currencies.AED, "comment2", default, default)},
            new object[] {new PaymentLinkData(55000, "rokfeller@bank.com", ServiceTypes.HTL, Currencies.EUR, "comment3", default, default)},
            new object[] {new PaymentLinkData((decimal) 77.77, "lucky@fortune.en", ServiceTypes.TRN, Currencies.AED, "comment4", default, default)},
            new object[] {new PaymentLinkData((decimal) 0.01, "minimal@techno.com", ServiceTypes.HTL, Currencies.EUR, "comment5", default, default)}
        };

        public static readonly object[][] InvalidLinkDataList =
        {
            new object[] {new PaymentLinkData(-121, "hit@yy.com", ServiceTypes.HTL, Currencies.EUR, "Invalid amount", default, default)},
            new object[] {new PaymentLinkData((decimal) 433.1, "antuan.com", ServiceTypes.TRN, Currencies.AED, "Invalid email", default, default)},
            new object[] {new PaymentLinkData(55000, "rokfeller@bank.com", ServiceTypes.HTL, Currencies.NotSpecified, "Unspecified currency", default, default)}
        };

        public static readonly object[][] LinkDataConflictingWithSettings =
        {
            new object[] {new PaymentLinkData(121, "hit@yy.com", ServiceTypes.TRN, Currencies.AED, "Not allowed service type", default, default)},
            new object[] {new PaymentLinkData((decimal) 433.1, "antuan@xor.com", ServiceTypes.TRN, Currencies.EUR, "Not allowed currency", default, default)}
        };
    }
}