using System.Globalization;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.MailSender.Formatters
{
    public static class PaymentAmountFormatter
    {
        public static string ToCurrencyString(decimal amount, Currencies currency)
        {
            switch (currency)
            {
                case Currencies.USD:
                    return Format(amount, "en-US");
                case Currencies.EUR:
                    return Format(amount, "de-DE");
                case Currencies.AED:
                    return Format(amount, "ar-SA");
                case Currencies.SAR:
                    return Format(amount, "ar-AE");
                default:
                case Currencies.NotSpecified:
                    return $"{amount:F2}";
            }


            string Format(decimal value, string culture) 
                => string.Format(new CultureInfo(culture), "{0:C}", value);
        }
    }
}
