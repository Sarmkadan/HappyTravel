using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Management.Enums;
using HappyTravel.Edo.Api.Models.Markups;
using HappyTravel.Edo.Api.Services.Customers;
using HappyTravel.Edo.Api.Services.Management;
using HappyTravel.Edo.Common.Enums.Markup;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Markup;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Edo.Api.Services.Markups
{
    public class MarkupPolicyManager : IMarkupPolicyManager
    {
        public MarkupPolicyManager(EdoContext context,
            ICustomerContext customerContext,
            IAdministratorContext administratorContext,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _customerContext = customerContext;
            _administratorContext = administratorContext;
            _dateTimeProvider = dateTimeProvider;
        }


        public Task<Result> Add(MarkupPolicyData policyData)
        {
            return ValidatePolicy(policyData)
                .OnSuccess(CheckPermissions)
                .OnSuccess(SavePolicy);

            Task<Result> CheckPermissions() => CheckUserManagePermissions(policyData.Scope);


            async Task<Result> SavePolicy()
            {
                var now = _dateTimeProvider.UtcNow();
                var (type, companyId, branchId, customerId) = policyData.Scope;

                var policy = new MarkupPolicy
                {
                    Description = policyData.Settings.Description,
                    Order = policyData.Settings.Order,
                    ScopeType = type,
                    Target = policyData.Target,
                    BranchId = branchId,
                    CompanyId = companyId,
                    CustomerId = customerId,
                    TemplateSettings = policyData.Settings.TemplateSettings,
                    Currency = policyData.Settings.Currency,
                    Created = now,
                    Modified = now,
                    TemplateId = policyData.Settings.TemplateId
                };

                _context.MarkupPolicies.Add(policy);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
        }


        public Task<Result> Remove(int policyId)
        {
            return GetPolicy()
                .OnSuccess(CheckPermissions)
                .OnSuccess(DeletePolicy);


            async Task<Result<MarkupPolicy>> GetPolicy()
            {
                var policy = await _context.MarkupPolicies.SingleOrDefaultAsync(p => p.Id == policyId);
                if (policy == null)
                    return Result.Fail<MarkupPolicy>("Could not find policy");

                return Result.Ok(policy);
            }


            async Task<Result<MarkupPolicy>> CheckPermissions(MarkupPolicy policy)
            {
                var scopeType = policy.ScopeType;
                var scope = new MarkupPolicyScope(scopeType,
                    policy.CompanyId ?? policy.BranchId ?? policy.CustomerId);

                var (_, isFailure, error) = await CheckUserManagePermissions(scope);
                if (isFailure)
                    return Result.Fail<MarkupPolicy>(error);

                return Result.Ok(policy);
            }


            async Task<Result> DeletePolicy(MarkupPolicy policy)
            {
                _context.Remove(policy);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
        }


        public async Task<Result> Modify(int policyId, MarkupPolicySettings settings)
        {
            var policy = await _context.MarkupPolicies.SingleOrDefaultAsync(p => p.Id == policyId);
            if (policy == null)
                return Result.Fail("Could not find policy");

            return await Result.Ok()
                .OnSuccess(CheckPermissions)
                .OnSuccess(UpdatePolicy);


            Task<Result> CheckPermissions()
            {
                var scopeData = new MarkupPolicyScope(policy.ScopeType,
                    policy.CompanyId ?? policy.BranchId ?? policy.CustomerId);

                return CheckUserManagePermissions(scopeData);
            }


            async Task<Result> UpdatePolicy()
            {
                policy.Description = settings.Description;
                policy.Order = settings.Order;
                policy.TemplateId = settings.TemplateId;
                policy.TemplateSettings = settings.TemplateSettings;
                policy.Currency = settings.Currency;
                policy.Modified = _dateTimeProvider.UtcNow();

                var validateResult = await ValidatePolicy(GetPolicyData(policy));
                if (validateResult.IsFailure)
                    return validateResult;

                _context.Update(policy);
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
        }


        public async Task<Result<List<MarkupPolicyData>>> Get(MarkupPolicyScope scope)
        {
            var (_, isFailure, error) = await CheckUserManagePermissions(scope);
            if (isFailure)
                return Result.Fail<List<MarkupPolicyData>>(error);

            var policies = (await GetPoliciesForScope(scope))
                .Select(GetPolicyData)
                .ToList();

            return Result.Ok(policies);
        }


        private Task<List<MarkupPolicy>> GetPoliciesForScope(MarkupPolicyScope scope)
        {
            var (type, companyId, branchId, customerId) = scope;
            switch (type)
            {
                case MarkupPolicyScopeType.Global:
                {
                    return _context.MarkupPolicies
                        .Where(p => p.ScopeType == MarkupPolicyScopeType.Global)
                        .ToListAsync();
                }
                case MarkupPolicyScopeType.Company:
                {
                    return _context.MarkupPolicies
                        .Where(p => p.ScopeType == MarkupPolicyScopeType.Company && p.CompanyId == companyId)
                        .ToListAsync();
                }
                case MarkupPolicyScopeType.Branch:
                {
                    return _context.MarkupPolicies
                        .Where(p => p.ScopeType == MarkupPolicyScopeType.Company && p.BranchId == branchId)
                        .ToListAsync();
                }
                case MarkupPolicyScopeType.Customer:
                {
                    return _context.MarkupPolicies
                        .Where(p => p.ScopeType == MarkupPolicyScopeType.Company && p.CustomerId == customerId)
                        .ToListAsync();
                }
                default:
                {
                    return Task.FromResult(new List<MarkupPolicy>(0));
                }
            }
        }


        private async Task<Result> CheckUserManagePermissions(MarkupPolicyScope scope)
        {
            var hasAdminPermissions = await _administratorContext.HasPermission(AdministratorPermissions.MarkupManagement);
            if (hasAdminPermissions)
                return Result.Ok();

            var (_, isFailure, customerData, error) = await _customerContext.GetCustomerInfo();
            if (isFailure)
                return Result.Fail(error);

            var (type, companyId, branchId, customerId) = scope;
            switch (type)
            {
                case MarkupPolicyScopeType.Customer:
                {
                    var isMasterCustomerInUserCompany = customerData.CompanyId == companyId
                        && customerData.IsMaster;

                    return isMasterCustomerInUserCompany
                        ? Result.Ok()
                        : Result.Fail("Permission denied");
                }
                case MarkupPolicyScopeType.Branch:
                {
                    var branch = await _context.Branches
                        .SingleOrDefaultAsync(b => b.Id == branchId);

                    if (branch == null)
                        return Result.Fail("Could not find branch");

                    var isMasterCustomer = customerData.CompanyId == branch.CompanyId
                        && customerData.IsMaster;

                    return isMasterCustomer
                        ? Result.Ok()
                        : Result.Fail("Permission denied");
                }
                case MarkupPolicyScopeType.EndClient:
                {
                    return customerData.CustomerId == customerId
                        ? Result.Ok()
                        : Result.Fail("Permission denied");
                }
                default:
                    return Result.Fail("Permission denied");
            }
        }


        private static MarkupPolicyData GetPolicyData(MarkupPolicy policy)
        {
            return new MarkupPolicyData(policy.Target,
                new MarkupPolicySettings(policy.Description, policy.TemplateId, policy.TemplateSettings, policy.Order, policy.Currency),
                GetPolicyScope());


            MarkupPolicyScope GetPolicyScope()
            {
                // Policy can belong to company, branch or customer.
                var scopeId = policy.CompanyId ?? policy.BranchId ?? policy.CustomerId;
                return new MarkupPolicyScope(policy.ScopeType, scopeId);
            }
        }


        private Task<Result> ValidatePolicy(MarkupPolicyData policyData)
        {
            return Result.Ok()
                .Ensure(ScopeIsValid, "Invalid scope data")
                .Ensure(TargetIsValid, "Invalid policy target")
                .Ensure(PolicyOrderIsUniqueForScope, "Policy with same order is already defined");


            bool ScopeIsValid()
            {
                var scope = policyData.Scope;
                switch (scope.Type)
                {
                    case MarkupPolicyScopeType.Global:
                        return scope.ScopeId == null;
                    case MarkupPolicyScopeType.Company:
                    case MarkupPolicyScopeType.Branch:
                    case MarkupPolicyScopeType.Customer:
                    case MarkupPolicyScopeType.EndClient:
                        return scope.ScopeId != null;
                    default:
                        return false;
                }
            }


            bool TargetIsValid() => policyData.Target != MarkupPolicyTarget.NotSpecified;


            async Task<bool> PolicyOrderIsUniqueForScope()
            {
                var isSameOrderPolicyExist = (await GetPoliciesForScope(policyData.Scope))
                    .Any(p => p.Order == policyData.Settings.Order);

                return !isSameOrderPolicyExist;
            }
        }


        private readonly IAdministratorContext _administratorContext;

        private readonly EdoContext _context;
        private readonly ICustomerContext _customerContext;
        private readonly IDateTimeProvider _dateTimeProvider;
    }
}