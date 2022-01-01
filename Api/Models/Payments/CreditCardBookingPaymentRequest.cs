using System;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.EdoContracts.General.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments
{
    /// <summary>
    ///     Payment request
    /// </summary>
    public readonly struct CreditCardBookingPaymentRequest
    {
        [JsonConstructor]
        public CreditCardBookingPaymentRequest(DataProviders source, PaymentTokenInfo token, string securityCode, string itineraryNumber, long availabilityId, Guid agreementId )
        {
            Source = source;
            Token = token;
            SecurityCode = securityCode;
            ItineraryNumber = itineraryNumber;
            AvailabilityId = availabilityId;
            AgreementId = agreementId;
        }
        
        /// <summary>
        ///     Availability source from 1-st step results
        /// </summary>
        public DataProviders Source { get; }
        

        /// <summary>
        ///     Payment token
        /// </summary>
        public PaymentTokenInfo Token { get; }

        
        /// <summary>
        ///     Credit card security code
        /// </summary>
        public string SecurityCode { get; }
        
        
        /// <summary>
        ///    The itinerary number (code) to combine several orders in one pack.
        /// </summary>
        public string ItineraryNumber { get; }
        
        
        /// <summary>
        ///     Exact search availability id
        /// </summary>
        public long AvailabilityId { get; }
        
        
        /// <summary>
        ///     Identifier of chosen agreement.
        /// </summary>
        public Guid AgreementId { get; }
    }
}