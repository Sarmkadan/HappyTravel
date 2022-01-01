using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Edo.Api.Services.Accommodations
{
    public class DeadlineDetailsCache: IDeadlineDetailsCache
    {
        public DeadlineDetailsCache(IMemoryFlow flow)
        {
            _flow = flow;
        }


        public void Set(string agreementId, DeadlineDetails deadlineDetails)
        {
            _flow.Set(_flow.BuildKey(KeyPrefix, agreementId), deadlineDetails, ExpirationPeriod);
        }


        public Result<DeadlineDetails> Get(string agreementId)
        {
            var isValueExist = _flow.TryGetValue<DeadlineDetails>(_flow.BuildKey(KeyPrefix, agreementId), out var deadlineDetails);

            return isValueExist
                ? Result.Ok(deadlineDetails)
                : Result.Fail<DeadlineDetails>($"Could not find deadline details by '{agreementId}'");
        }


        private const string KeyPrefix = nameof(AvailabilityDetails) + "DeadlineDetails";
        private static readonly TimeSpan ExpirationPeriod = TimeSpan.FromHours(1);
        private readonly IMemoryFlow _flow;
    }
}