using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.MailSender
{
    public interface IMailSender
    {
        Task<Result> Send<TMessageData>(string templateId, string recipientAddress, TMessageData messageData);

        Task<Result> Send<TMessageData>(string templateId, IEnumerable<string> recipientAddresses, TMessageData messageData);
    }
}