using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Services.Connectors
{
    public interface IDataProvider
    {
        Task<Result<AvailabilityDetails, ProblemDetails>> GetAvailability(AvailabilityRequest availabilityRequest, string languageCode);

        Task<Result<SingleAccommodationAvailabilityDetails, ProblemDetails>> GetAvailability(long availabilityId,
            string accommodationId, string languageCode);

        Task<Result<DeadlineDetails, ProblemDetails>> GetDeadline(string accommodationId, string availabilityId, string agreementCode, string languageCode);

        Task<Result<AccommodationDetails, ProblemDetails>> GetAccommodation(string accommodationId, string languageCode);

        Task<Result<SingleAccommodationAvailabilityDetailsWithDeadline, ProblemDetails>> GetExactAvailability(long availabilityId, Guid agreementId,
            string languageCode);
    }
}