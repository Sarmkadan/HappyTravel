using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure.Options;
using HappyTravel.Edo.Api.Models.Payments;
using HappyTravel.MailSender;
using HappyTravel.MailSender.Formatters;
using Microsoft.Extensions.Options;
using static HappyTravel.MailSender.Formatters.EmailContentFormatter;

namespace HappyTravel.Edo.Api.Services.Payments
{
    public class PaymentNotificationService : IPaymentNotificationService
    {
        public PaymentNotificationService(IMailSender mailSender, IOptions<PaymentNotificationOptions> options)
        {
            _mailSender = mailSender;
            _options = options.Value;
        }


        public Task<Result> SendBillToCustomer(PaymentBill paymentBill)
        {
            var templateId = string.IsNullOrWhiteSpace(paymentBill.CustomerName)
                ? _options.UnknownCustomerTemplateId
                : _options.KnownCustomerTemplateId;

            var payload = new
            {
                amount = FromAmount(paymentBill.Amount, paymentBill.Currency),
                customerName = paymentBill.CustomerName,
                date = FromDateTime(paymentBill.Date),
                method = FromEnumDescription(paymentBill.Method),
                referenceCode = paymentBill.ReferenceCode
            };

            return _mailSender.Send(templateId, paymentBill.CustomerEmail, payload);
        }


        public Task<Result> SendNeedPaymentNotificationToCustomer(PaymentBill paymentBill)
            => _mailSender.Send(_options.NeedPaymentTemplateId, paymentBill.CustomerEmail, new
            {
                amount = PaymentAmountFormatter.ToCurrencyString(paymentBill.Amount, paymentBill.Currency),
                method = EnumFormatter.ToDescriptionString(paymentBill.Method),
                referenceCode = paymentBill.ReferenceCode,
                customerName = paymentBill.CustomerName
            });


        private readonly IMailSender _mailSender;
        private readonly PaymentNotificationOptions _options;
    }
}