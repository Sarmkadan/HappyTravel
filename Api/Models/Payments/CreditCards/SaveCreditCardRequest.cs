using HappyTravel.Edo.Common.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments.CreditCards
{
    /// <summary>
    ///     Request to save credit card token to database
    /// </summary>
    public readonly struct SaveCreditCardRequest
    {
        [JsonConstructor]
        public SaveCreditCardRequest(string number, string expirationDate, string holderName, string token, string referenceCode, CreditCardOwnerType ownerType)
        {
            Number = number;
            ExpirationDate = expirationDate;
            HolderName = holderName;
            Token = token;
            ReferenceCode = referenceCode;
            OwnerType = ownerType;
        }


        /// <summary>
        ///     Masked card number
        /// </summary>
        public string Number { get; }

        /// <summary>
        ///     Expiration date
        /// </summary>
        public string ExpirationDate { get; }

        /// <summary>
        ///     Card holder name
        /// </summary>
        public string HolderName { get; }

        /// <summary>
        ///     Card token
        /// </summary>
        public string Token { get; }

        /// <summary>
        ///     Reference code that was used while tokenization process
        /// </summary>
        public string ReferenceCode { get; }

        /// <summary>
        ///     Card owner type
        /// </summary>
        public CreditCardOwnerType OwnerType { get; }
    }
}