using System;
using HappyTravel.Edo.Common.Enums;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Payments
{
    /// <summary>
    ///     Payment request with account
    /// </summary>
    public readonly struct AccountBookingPaymentRequest
    {
        [JsonConstructor]
        public AccountBookingPaymentRequest(DataProviders source, string itineraryNumber, long availabilityId, Guid agreementId)
        {
            Source = source;
            ItineraryNumber = itineraryNumber;
            AvailabilityId = availabilityId;
            AgreementId = agreementId;
        }

        /// <summary>
        ///     Availability source from 1-st step results
        /// </summary>
        public DataProviders Source { get; }

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