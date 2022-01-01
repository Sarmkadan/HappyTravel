using System;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.Edo.Api.Models.Payments
{
    public readonly struct PaymentBill
    {
        public PaymentBill(string customerEmail, decimal amount, Currencies currency,
            DateTime date, PaymentMethods method, string referenceCode, string clientName = default)
        {
            CustomerEmail = customerEmail;
            Amount = amount;
            Currency = currency;
            Date = date;
            Method = method;
            ReferenceCode = referenceCode;
            CustomerName = clientName ?? string.Empty;
        }


        public string CustomerEmail { get; }
        public decimal Amount { get; }
        public Currencies Currency { get; }
        public DateTime Date { get; }
        public PaymentMethods Method { get; }
        public string ReferenceCode { get; }
        public string CustomerName { get; }
    }
}