using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Api.Models.Markups;
using HappyTravel.Edo.Api.Services.CurrencyConversion;
using HappyTravel.Edo.Api.Services.Customers;
using HappyTravel.Edo.Api.Services.Markups.Templates;
using HappyTravel.Edo.Common.Enums.Markup;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Markup;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Edo.Api.Services.Markups
{
    public class MarkupService : IMarkupService
    {
        public MarkupService(EdoContext context, IMemoryFlow memoryFlow,
            IMarkupPolicyTemplateService templateService,
            ICurrencyRateService currencyRateService,
            ICustomerSettingsManager customerSettingsManager)
        {
            _context = context;
            _memoryFlow = memoryFlow;
            _templateService = templateService;
            _currencyRateService = currencyRateService;
            _customerSettingsManager = customerSettingsManager;
        }


        public async Task<Markup> Get(CustomerInfo customerInfo, MarkupPolicyTarget policyTarget)
        {
            var (_, _, settings, _) = await _customerSettingsManager.GetUserSettings(customerInfo);
            var customerPolicies = await GetCustomerPolicies(customerInfo, settings, policyTarget);
            var markupFunction = CreateAggregatedMarkupFunction(customerPolicies);
            return new Markup
            {
                Policies = customerPolicies,
                Function = markupFunction
            };
        }


        private ValueTask<List<MarkupPolicy>> GetCustomerPolicies(CustomerInfo customerInfo, CustomerUserSettings userSettings,
            MarkupPolicyTarget policyTarget)
        {
            var (customerId, companyId, branchId, _) = customerInfo;

            return _memoryFlow.GetOrSetAsync(BuildKey(),
                GetOrderedPolicies,
                CustomerPoliciesCachingTime);


            string BuildKey()
                => _memoryFlow.BuildKey(nameof(MarkupService),
                    "MarkupPolicies",
                    customerId.ToString());


            async Task<List<MarkupPolicy>> GetOrderedPolicies()
            {
                var policiesFromDb = await GetPoliciesFromDb();
                return policiesFromDb.Where(FilterBySettings)
                    .OrderBy(SortByScope)
                    .ThenBy(p => p.Order)
                    .ToList();
            }


            bool FilterBySettings(MarkupPolicy policy)
            {
                if (policy.ScopeType == MarkupPolicyScopeType.EndClient && !userSettings.IsEndClientMarkupsEnabled)
                    return false;

                return true;
            }


            int SortByScope(MarkupPolicy policy) => (int) policy.ScopeType;


            Task<List<MarkupPolicy>> GetPoliciesFromDb()
            {
                return _context.MarkupPolicies
                    .Where(p => p.Target == policyTarget)
                    .Where(p =>
                        p.ScopeType == MarkupPolicyScopeType.Global ||
                        p.ScopeType == MarkupPolicyScopeType.Company && p.CompanyId == companyId ||
                        p.ScopeType == MarkupPolicyScopeType.Branch && p.BranchId == branchId ||
                        p.ScopeType == MarkupPolicyScopeType.Customer && p.CustomerId == customerId ||
                        p.ScopeType == MarkupPolicyScopeType.EndClient && p.CustomerId == customerId
                    )
                    .ToListAsync();
            }
        }


        private AggregatedMarkupFunction CreateAggregatedMarkupFunction(List<MarkupPolicy> policies)
        {
            var markupPolicyFunctions = policies
                .Select(GetPolicyFunction)
                .ToList();

            // TODO: rewrite to async streams after migrating to .NET Core 3
            return async (supplierPrice, currency) =>
            {
                var price = supplierPrice;
                foreach (var markupPolicyFunction in markupPolicyFunctions)
                {
                    var currencyRate = await _currencyRateService.Get(currency, markupPolicyFunction.Currency);
                    price = markupPolicyFunction.Function(price * currencyRate) / currencyRate;
                }

                return price;
            };
        }


        private MarkupPolicyFunction GetPolicyFunction(MarkupPolicy policy)
        {
            return _memoryFlow
                .GetOrSet(BuildKey(policy),
                    () =>
                    {
                        return new MarkupPolicyFunction
                        {
                            Currency = policy.Currency,
                            Function = _templateService
                                .CreateFunction(policy.TemplateId, policy.TemplateSettings)
                        };
                    },
                    MarkupPolicyFunctionCachingTime);


            string BuildKey(MarkupPolicy policyWithFunc)
                => _memoryFlow.BuildKey(nameof(MarkupService),
                    "Functions",
                    policyWithFunc.Id.ToString(),
                    policyWithFunc.Modified.ToString(CultureInfo.InvariantCulture));
        }


        private static readonly TimeSpan MarkupPolicyFunctionCachingTime = TimeSpan.FromDays(1);
        private static readonly TimeSpan CustomerPoliciesCachingTime = TimeSpan.FromMinutes(5);
        private readonly EdoContext _context;
        private readonly ICurrencyRateService _currencyRateService;
        private readonly ICustomerSettingsManager _customerSettingsManager;
        private readonly IMemoryFlow _memoryFlow;
        private readonly IMarkupPolicyTemplateService _templateService;
    }
}