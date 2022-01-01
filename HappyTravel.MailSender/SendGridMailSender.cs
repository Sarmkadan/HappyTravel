using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.MailSender.Infrastructure;
using HappyTravel.MailSender.Infrastructure.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HappyTravel.MailSender
{
    public class SendGridMailSender : IMailSender
    {
        public SendGridMailSender(IOptions<SenderOptions> senderOptions, IHttpClientFactory httpClientFactory, ILogger<SendGridMailSender> logger)
        {
            _senderOptions = senderOptions.Value;
            _logger = logger ?? new NullLogger<SendGridMailSender>();
            _httpClientFactory = httpClientFactory;
            
            if(string.IsNullOrWhiteSpace(_senderOptions.ApiKey))
                throw new ArgumentNullException(nameof(_senderOptions.ApiKey));
            
            if(string.IsNullOrWhiteSpace(_senderOptions.BaseUrl))
                throw new ArgumentNullException(nameof(_senderOptions.BaseUrl));
            
            if(_senderOptions.SenderAddress == default)
                throw new ArgumentNullException(nameof(_senderOptions.SenderAddress));
        }


        public Task<Result> Send<TMessageData>(string templateId, string recipientAddress, TMessageData messageData)
            => Send(templateId, new[] {recipientAddress}, messageData);


        public async Task<Result> Send<TMessageData>(string templateId, IEnumerable<string> recipientAddresses, TMessageData messageData)
        {
            var enumerable = recipientAddresses as string[] ?? recipientAddresses.ToArray();
            if (!enumerable.Any())
                return Result.Fail("No recipient addresses provided");

            var templateData = GetTemplateData(templateId, messageData);
            using (var httpClient = _httpClientFactory.CreateClient(HttpClientName))
            {
                var client = new SendGridClient(httpClient, _senderOptions.ApiKey);
                try
                {
                    var result = Result.Ok();
                    foreach (var address in enumerable)
                    {
                        var message = new SendGridMessage
                        {
                            TemplateId = templateId,
                            From = _senderOptions.SenderAddress
                        };

                        message.SetTemplateData(templateData);
                        message.AddTo(address);

                        var response = await client.SendEmailAsync(message);
                        if (response.StatusCode == HttpStatusCode.Accepted)
                        {
                            _logger.LogSendMailInformation($"{templateId} successfully e-mailed to {address}");
                        }
                        else
                        {
                            var error = await response.Body.ReadAsStringAsync();
                            var failure =
                                $"Could not send an e-mail {templateId} to {address}, a server responded: '{error}' with status code '{response.StatusCode}'";
                            result = Result.Combine(result, Result.Fail(failure));

                            _logger.LogSendMailError(failure);
                        }

                        result = Result.Combine(result, Result.Ok());
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    _logger.LogSendMailException(ex);
                    return Result.Fail("Unhandled error occured while sending an e-mail.");
                }
            }
        }


        public static string HttpClientName = "SendGrid";


        private PropertyInfo[] GetProperties<TMessageData>(string templateId, TMessageData messageData)
        {
            if (_templateProperties.TryGetValue(templateId, out var properties))
                return properties;

            properties = messageData.GetType().GetProperties();
            _templateProperties.TryAdd(templateId, properties);

            return properties;
        }


        private IDictionary<string, object> GetTemplateData<TMessageData>(string templateId, TMessageData messageData)
        {
            var templateData = new ExpandoObject() as IDictionary<string, object>;
            templateData[_senderOptions.BaseUrlTemplateName] = _senderOptions.BaseUrl;
            if (messageData == null)
                return templateData;

            foreach (var propertyInfo in GetProperties(templateId, messageData))
                templateData[propertyInfo.Name] = propertyInfo.GetValue(messageData, null);

            return templateData;
        }


        private readonly ConcurrentDictionary<string, PropertyInfo[]> _templateProperties = new ConcurrentDictionary<string, PropertyInfo[]>();

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SendGridMailSender> _logger;
        private readonly SenderOptions _senderOptions;
    }
}