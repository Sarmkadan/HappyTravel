using System.ComponentModel.DataAnnotations;
using HappyTravel.EdoContracts.General.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments
{
    public readonly struct PaymentData
    {
        [JsonConstructor]
        public PaymentData(decimal amount, Currencies currency, string reason)
        {
            Amount = amount;
            Currency = currency;
            Reason = reason;
        }


        /// <summary>
        ///     Money amount.
        /// </summary>
        [Required]
        public decimal Amount { get; }

        /// <summary>
        ///     Payment currency.
        /// </summary>
        [Required]
        public Currencies Currency { get; }

        /// <summary>
        ///     Reason for payment.
        /// </summary>
        [Required]
        public string Reason { get; }
    }
}