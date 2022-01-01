using System;
using Microsoft.Extensions.Logging;

namespace HappyTravel.MailSender.Infrastructure.Logging
{
    internal static class LoggerExtensions
    {
        static LoggerExtensions()
        {
            SendMailEventOccured = LoggerMessage.Define<string>(LogLevel.Information,
                GetEventId(LoggerEvents.SendMailInformation),
                $"INFORMATION | MailSender: {{message}}");
            SendMailExceptionOccurred = LoggerMessage.Define(LogLevel.Critical,
                GetEventId(LoggerEvents.SendMailException),
                $"EXCEPTION | MailSender: ");
            SendMailErrorOccured = LoggerMessage.Define<string>(LogLevel.Error,
                GetEventId(LoggerEvents.SendMailInformation),
                $"INFORMATION | MailSender: {{message}}");
        }


        internal static void LogSendMailError(this ILogger logger, string message) 
            => SendMailErrorOccured(logger, message, null);


        internal static void LogSendMailException(this ILogger logger, Exception exception) 
            => SendMailExceptionOccurred(logger, exception);


        internal static void LogSendMailInformation(this ILogger logger, string message) 
            => SendMailEventOccured(logger, message, null);


        private static EventId GetEventId(LoggerEvents @event) 
            => new EventId((int) @event, @event.ToString());


        private static readonly Action<ILogger, string, Exception> SendMailEventOccured;
        private static readonly Action<ILogger, string, Exception> SendMailErrorOccured;
        private static readonly Action<ILogger, Exception> SendMailExceptionOccurred;
    }
}