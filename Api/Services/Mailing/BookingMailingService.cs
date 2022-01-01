using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentValidation;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Infrastructure.Options;
using HappyTravel.Edo.Api.Services.Accommodations;
using HappyTravel.MailSender;
using Microsoft.Extensions.Options;

namespace HappyTravel.Edo.Api.Services.Mailing
{
    public class BookingMailingService : IBookingMailingService
    {
        public BookingMailingService(IMailSender mailSender,
            IBookingDocumentsService bookingDocumentsService,
            IOptions<BookingMailingOptions> options)
        {
            _bookingDocumentsService = bookingDocumentsService;
            _mailSender = mailSender;
            _options = options.Value;
        }


        public Task<Result> SendVoucher(int bookingId, string email)
        {
            return SendEmail(email,
                _options.VoucherTemplateId,
                () => _bookingDocumentsService.GenerateVoucher(bookingId));
        }


        public Task<Result> SendInvoice(int bookingId, string email)
        {
            return SendEmail(email,
                _options.InvoiceTemplateId,
                () => _bookingDocumentsService.GenerateInvoice(bookingId));
        }


        public Task<Result> NotifyBookingCancelled(string referenceCode, string email, string customerName)
            => _mailSender.Send(_options.BookingCancelledTemplateId, email, new
            {
                customerName,
                referenceCode
            });


        private Task<Result> SendEmail<T>(string email, string templateId, Func<Task<Result<T>>> getSendDataFunction)
        {
            return Validate()
                .OnSuccess(getSendDataFunction)
                .OnSuccess(Send);

            Result Validate() => GenericValidator<string>.Validate(setup => setup.RuleFor(e => e).EmailAddress(), email);

            async Task<Result> Send(T data) => await _mailSender.Send(templateId, email, data);
        }


        private readonly IBookingDocumentsService _bookingDocumentsService;


        private readonly IMailSender _mailSender;
        private readonly BookingMailingOptions _options;
    }
}