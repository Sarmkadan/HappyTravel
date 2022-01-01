using System.ComponentModel;

namespace HappyTravel.Edo.Api.Infrastructure.Converters.EnumConverters
{
    public static class Currencies
    {
        internal static string ToCurrencyCode(EdoContracts.General.Enums.Currencies currency)
        {
            switch (currency)
            {
                case EdoContracts.General.Enums.Currencies.AED:
                    return "AED";
                case EdoContracts.General.Enums.Currencies.SAR:
                    return  "SAR";
                case EdoContracts.General.Enums.Currencies.USD:
                    return  "USD";
                case EdoContracts.General.Enums.Currencies.EUR:
                    return "EUR";
                case EdoContracts.General.Enums.Currencies.NotSpecified:
                    return "NotSpecified";
                default:
                    throw new InvalidEnumArgumentException($"Unknown currency: {currency}");
            }
        }
    }
}