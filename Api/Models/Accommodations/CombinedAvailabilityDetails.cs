using System;
using System.Collections.Generic;

namespace HappyTravel.Edo.Api.Models.Accommodations
{
    public readonly struct CombinedAvailabilityDetails
    {
        public CombinedAvailabilityDetails(int numberOfNights, DateTime checkInDate, DateTime checkOutDate, int numberOfProcessedResults, List<ProviderData<AvailabilityResult>> results)
        {
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            NumberOfNights = numberOfNights;
            NumberOfProcessedResults = numberOfProcessedResults;
            Results = results;
        }

        /// <summary>
        /// Number of nights
        /// </summary>
        public int NumberOfNights { get; }
        
        /// <summary>
        /// Check-in date
        /// </summary>
        public DateTime CheckInDate { get; }
        
        /// <summary>
        /// Check-out date
        /// </summary>
        public DateTime CheckOutDate { get; }

        /// <summary>
        /// Number of all processed accommodations
        /// </summary>
        public int NumberOfProcessedResults { get; }

        /// <summary>
        /// Availability results, grouped by accommodation
        /// </summary>
        public List<ProviderData<AvailabilityResult>> Results { get; }
    }
}