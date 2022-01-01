using System;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.Edo.Data.CurrencyExchange
{
    public class CurrencyRate
    {
        public Currencies SourceCurrency { get; set; }
        public Currencies TargetCurrency { get; set; }
        public decimal Rate { get; set; }
        
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}