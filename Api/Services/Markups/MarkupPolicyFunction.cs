using System;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.Edo.Api.Services.Markups
{
    public class MarkupPolicyFunction
    {
        public Currencies Currency { get; set; }
        public Func<decimal, decimal> Function { get; set; }
    }
}