using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Services.Deadline
{
    public interface ICancellationPoliciesService
    {
        Task<Result<DeadlineDetails, ProblemDetails>> GetDeadlineDetails(string availabilityId,
            string accommodationId,
            string tariffCode,
            DataProviders dataProvider,
            string languageCode);
    }
}