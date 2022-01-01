namespace HappyTravel.Edo.Api.Infrastructure.Options
{
    public class PaymentNotificationOptions
    {
        public string UnknownCustomerTemplateId { get; set; }
        public string KnownCustomerTemplateId { get; set; }
        public string NeedPaymentTemplateId { get; set; }
    }
}