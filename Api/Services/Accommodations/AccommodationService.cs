using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.Edo.Api.Services.Connectors;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Services.Accommodations
{
    public class AccommodationService : IAccommodationService
    {
        public AccommodationService(IMemoryFlow flow,
            IProviderRouter providerRouter)
        {
            _flow = flow;
            _providerRouter = providerRouter;
        }


        public ValueTask<Result<AccommodationDetails, ProblemDetails>> Get(DataProviders source, string accommodationId, string languageCode)
        {
            return _flow.GetOrSetAsync(_flow.BuildKey(nameof(AccommodationService), "Accommodations", languageCode, accommodationId),
                async () => await _providerRouter.GetAccommodation(source, accommodationId, languageCode),
                TimeSpan.FromDays(1));
        }


        private readonly IMemoryFlow _flow;
        private readonly IProviderRouter _providerRouter;
    }
}