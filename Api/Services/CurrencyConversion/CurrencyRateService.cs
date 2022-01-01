using System;
using System.Threading.Tasks;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.CurrencyExchange;
using HappyTravel.EdoContracts.General.Enums;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Edo.Api.Services.CurrencyConversion
{
    public class CurrencyRateService : ICurrencyRateService
    {
        public CurrencyRateService(EdoContext context,
            IDateTimeProvider dateTimeProvider,
            IMemoryFlow memoryFlow)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _memoryFlow = memoryFlow;
        }


        public async Task Set(Currencies source, Currencies target, decimal rate)
        {
            var now = _dateTimeProvider.UtcNow();
            var currentRate = await GetCurrent(source, target);
            if (currentRate != default)
            {
                currentRate.ValidTo = _dateTimeProvider.UtcNow();
                _context.CurrencyRates.Update(currentRate);
            }

            _context.CurrencyRates.Add(CreateRate());
            await _context.SaveChangesAsync();


            CurrencyRate CreateRate()
                => new CurrencyRate
                {
                    SourceCurrency = source,
                    TargetCurrency = target,
                    Rate = rate,
                    ValidFrom = now
                };
        }


        public ValueTask<decimal> Get(Currencies source, Currencies target)
        {
            if (source == target)
                return SameCurrencyRateResult;

            // TODO: remove this when currency conversion will be implemented
            return SameCurrencyRateResult;

            var key = _memoryFlow.BuildKey(nameof(CurrencyRateService), source.ToString(), target.ToString());
            return _memoryFlow.GetOrSetAsync(key, async () =>
            {
                var rt = await GetCurrent(source, target);
                return rt.Rate;
            }, TimeSpan.FromMinutes(5));
        }


        private Task<CurrencyRate> GetCurrent(Currencies source, Currencies target)
        {
            return _context.CurrencyRates.SingleOrDefaultAsync(cr => cr.SourceCurrency == source &&
                cr.TargetCurrency == target && cr.ValidTo == null);
        }


        private static readonly ValueTask<decimal> SameCurrencyRateResult = new ValueTask<decimal>(1);
        private readonly EdoContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMemoryFlow _memoryFlow;
    }
}