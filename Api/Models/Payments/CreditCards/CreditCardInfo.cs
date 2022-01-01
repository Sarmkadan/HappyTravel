using HappyTravel.Edo.Common.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments.CreditCards
{
    /// <summary>
    ///     Saved credit card info
    /// </summary>
    public readonly struct CreditCardInfo
    {
        [JsonConstructor]
        public CreditCardInfo(int id, string number, string expirationDate, string holderName, CreditCardOwnerType ownerType, PaymentTokenInfo token)
        {
            Id = id;
            Number = number;
            ExpirationDate = expirationDate;
            HolderName = holderName;
            OwnerType = ownerType;
            Token = token;
        }


        /// <summary>
        ///     Card identifier
        /// </summary>
        public int Id { get; }

        /// <summary>
        ///     Masked card number
        /// </summary>
        public string Number { get; }

        /// <summary>
        ///     Expiration Date
        /// </summary>
        public string ExpirationDate { get; }

        /// <summary>
        ///     Card holder name
        /// </summary>
        public string HolderName { get; }

        /// <summary>
        ///     Card owner type
        /// </summary>
        public CreditCardOwnerType OwnerType { get; }

        /// <summary>
        ///     Payment token
        /// </summary>
        public PaymentTokenInfo Token { get; }
    }
}