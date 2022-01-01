using System;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.MailSender.Formatters
{
    public static class EmailContentFormatter
    {
        public static string FromAmount(decimal amount, Currencies currency) 
            => PaymentAmountFormatter.ToCurrencyString(amount, currency);


        public static string FromDateTime(DateTime dateTime) => 
            $"{dateTime:yyyy.MM.dd hh:mm} UTC";


        public static string FromEnumDescription<T>(T value) where T : Enum 
            => EnumFormatter.ToDescriptionString(value);
    }
}
