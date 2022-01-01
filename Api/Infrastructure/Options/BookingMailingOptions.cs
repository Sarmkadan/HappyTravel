namespace HappyTravel.Edo.Api.Infrastructure.Options
{
    public class BookingMailingOptions
    {
        public string VoucherTemplateId { get; set; }
        public string InvoiceTemplateId { get; set; }
        public string BookingCancelledTemplateId { get; set; }
    }
}