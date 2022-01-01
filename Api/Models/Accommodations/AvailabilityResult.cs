using System.Collections.Generic;
using HappyTravel.EdoContracts.Accommodations.Internals;

namespace HappyTravel.Edo.Api.Models.Accommodations
{
    public readonly struct AvailabilityResult
    {
        public AvailabilityResult(long availabilityId, SlimAccommodationDetails accommodationDetails, List<Agreement> agreements)
        {
            AvailabilityId = availabilityId;
            AccommodationDetails = accommodationDetails;
            Agreements = agreements ?? new List<Agreement>(0);
        }
        
        /// <summary>
        /// Id of availability search
        /// </summary>
        public long AvailabilityId { get; }
        
        /// <summary>
        /// Accommodation data
        /// </summary>
        public SlimAccommodationDetails AccommodationDetails { get; }
        
        /// <summary>
        /// List of available agreements
        /// </summary>
        public List<Agreement> Agreements { get; }
    }
}