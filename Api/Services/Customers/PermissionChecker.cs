using System;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FloxDc.CacheFlow;
using FloxDc.CacheFlow.Extensions;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public class PermissionChecker : IPermissionChecker
    {
        public PermissionChecker(EdoContext context, IMemoryFlow flow)
        {
            _context = context;
            _flow = flow;
        }


        public async ValueTask<Result> CheckInCompanyPermission(CustomerInfo customer, InCompanyPermissions permission)
        {
            var isCompanyVerified = await _flow.GetOrSetAsync(BuildKey(customer.CompanyId), () =>
            {
                return _context.Companies
                    .Where(c => c.Id == customer.CompanyId)
                    .Select(c => c.State == CompanyStates.Verified)
                    .SingleOrDefaultAsync();
            }, VerifiedCompaniesCacheTtl);

            if (!isCompanyVerified)
                return Result.Fail("Action is available only for verified companies");

            return customer.InCompanyPermissions.HasFlag(permission)
                ? Result.Ok()
                : Result.Fail($"Customer does not have permission '{permission}'");


            string BuildKey(int companyId)
            {
                const string keyPrefix = nameof(PermissionChecker) + "VerifiedCompanies";
                return _flow.BuildKey(keyPrefix, companyId.ToString());
            }
        }


        public async ValueTask<Result> CheckInCompanyPermission(int customerId, int companyId, InCompanyPermissions permission)
        {
            var relationData = await _context.CustomerCompanyRelations
                .Where(i => i.CustomerId == customerId)
                .Where(i => i.CompanyId == companyId).SingleOrDefaultAsync();

            if (Equals(relationData, default))
                return Result.Fail("The customer isn't affiliated with the company");

            if (!relationData.InCompanyPermissions.HasFlag(permission))
                return Result.Fail($"Customer does not have permission '{permission}'");

            return Result.Ok();
        }


        private static readonly TimeSpan VerifiedCompaniesCacheTtl = TimeSpan.FromMinutes(5);


        private readonly EdoContext _context;
        private readonly IMemoryFlow _flow;
    }
}