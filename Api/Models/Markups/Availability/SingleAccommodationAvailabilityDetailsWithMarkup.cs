using System.Collections.Generic;
using HappyTravel.Edo.Data.Markup;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Edo.Api.Models.Markups.Availability
{
    public readonly struct SingleAccommodationAvailabilityDetailsWithMarkup
    {
        public SingleAccommodationAvailabilityDetailsWithMarkup(List<MarkupPolicy> appliedPolicies, SingleAccommodationAvailabilityDetails resultResponse)
        {
            AppliedPolicies = appliedPolicies;
            ResultResponse = resultResponse;
        }


        public List<MarkupPolicy> AppliedPolicies { get; }
        public SingleAccommodationAvailabilityDetails ResultResponse { get; }
    }
}