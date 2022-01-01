using SendGrid.Helpers.Mail;

namespace HappyTravel.MailSender.Infrastructure
{
    public class SenderOptions
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public string BaseUrlTemplateName { get; set; } = "baseUrl";
        public EmailAddress SenderAddress { get; set; }
    }
}
