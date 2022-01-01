using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentValidation;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Infrastructure.FunctionalExtensions;
using HappyTravel.Edo.Api.Models.Branches;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Api.Models.Management.AuditEvents;
using HappyTravel.Edo.Api.Models.Management.Enums;
using HappyTravel.Edo.Api.Services.Management;
using HappyTravel.Edo.Api.Services.Payments;
using HappyTravel.Edo.Api.Services.Payments.Accounts;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Customers;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public class CompanyService : ICompanyService
    {
        public CompanyService(EdoContext context,
            IAccountManagementService accountManagementService,
            IAdministratorContext administratorContext,
            IDateTimeProvider dateTimeProvider,
            IManagementAuditService managementAuditService,
            ICustomerContext customerContext)
        {
            _context = context;
            _accountManagementService = accountManagementService;
            _administratorContext = administratorContext;
            _dateTimeProvider = dateTimeProvider;
            _managementAuditService = managementAuditService;
            _customerContext = customerContext;
        }


        public async Task<Result<Company>> Add(CompanyRegistrationInfo company)
        {
            var (_, isFailure, error) = Validate(company);
            if (isFailure)
                return Result.Fail<Company>(error);

            var now = _dateTimeProvider.UtcNow();
            var createdCompany = new Company
            {
                Address = company.Address,
                City = company.City,
                CountryCode = company.CountryCode,
                Fax = company.Fax,
                Name = company.Name,
                Phone = company.Phone,
                Website = company.Website,
                PostalCode = company.PostalCode,
                PreferredCurrency = company.PreferredCurrency,
                PreferredPaymentMethod = company.PreferredPaymentMethod,
                State = CompanyStates.PendingVerification,
                Created = now,
                Updated = now
            };

            _context.Companies.Add(createdCompany);
            await _context.SaveChangesAsync();
            
            var defaultBranch = new Branch
            {
                Title = createdCompany.Name,
                CompanyId = createdCompany.Id,
                IsDefault = true,
                Created = now,
                Modified = now,
            };
            _context.Branches.Add(defaultBranch);
            
            await _context.SaveChangesAsync();
            return Result.Ok(createdCompany);
        }


        public Task<Result<Branch>> AddBranch(int companyId, BranchInfo branch)
        {
            return CheckCompanyExists()
                .Ensure(HasPermissions, "Permission to create branches denied")
                .Ensure(BranchTitleIsUnique, $"Branch with title {branch.Title} already exists")
                .OnSuccess(SaveBranch);


            async Task<bool> HasPermissions()
            {
                var (_, isFailure, customerInfo, _) = await _customerContext.GetCustomerInfo();
                if (isFailure)
                    return false;

                return customerInfo.IsMaster && customerInfo.CompanyId == companyId;
            }


            async Task<Result> CheckCompanyExists()
            {
                return await _context.Companies.AnyAsync(c => c.Id == companyId)
                    ? Result.Ok()
                    : Result.Fail("Could not find company with specified id");
            }


            async Task<bool> BranchTitleIsUnique()
            {
                return !await _context.Branches.Where(b => b.CompanyId == companyId &&
                        EF.Functions.ILike(b.Title, branch.Title))
                    .AnyAsync();
            }

            
            async Task<Branch> SaveBranch()
            {
                var now = _dateTimeProvider.UtcNow();
                var createdBranch = new Branch
                {
                    Title = branch.Title,
                    CompanyId = companyId,
                    IsDefault = false,
                    Created = now,
                    Modified = now,
                };
                _context.Branches.Add(createdBranch);
                await _context.SaveChangesAsync();

                return createdBranch;
            }
        }
        
        
        public Task<Branch> GetDefaultBranch(int companyId)
        {
            return _context.Branches
                .SingleAsync(b => b.CompanyId == companyId && b.IsDefault);
        }


        public Task<Result> SetVerified(int companyId, string verifyReason)
        {
            var now = _dateTimeProvider.UtcNow();
            return Result.Ok()
                .Ensure(HasVerifyRights, "Permission denied")
                .OnSuccess(GetCompany)
                .OnSuccessWithTransaction(_context, company => Result.Ok(company)
                    .OnSuccess(SetCompanyVerified)
                    .OnSuccess(CreatePaymentAccount)
                    .OnSuccess(WriteAuditLog));

            Task<bool> HasVerifyRights() => _administratorContext.HasPermission(AdministratorPermissions.CompanyVerification);


            async Task<Result<Company>> GetCompany()
            {
                var company = await _context.Companies.SingleOrDefaultAsync(c => c.Id == companyId);
                return company == default
                    ? Result.Fail<Company>($"Could not find company with id {companyId}")
                    : Result.Ok(company);
            }


            Task SetCompanyVerified(Company company)
            {
                company.State = CompanyStates.Verified;
                company.VerificationReason = verifyReason;
                company.Verified = now;
                company.Updated = now;
                _context.Update(company);
                return _context.SaveChangesAsync();
            }


            Task<Result> CreatePaymentAccount(Company company)
                => _accountManagementService
                    .Create(company, company.PreferredCurrency);


            Task WriteAuditLog()
                => _managementAuditService.Write(ManagementEventType.CompanyVerification,
                    new CompanyVerifiedAuditEventData(companyId, verifyReason));
        }


        private Result Validate(in CompanyRegistrationInfo companyRegistration)
        {
            return GenericValidator<CompanyRegistrationInfo>.Validate(v =>
            {
                v.RuleFor(c => c.Name).NotEmpty();
                v.RuleFor(c => c.Address).NotEmpty();
                v.RuleFor(c => c.City).NotEmpty();
                v.RuleFor(c => c.Phone).NotEmpty().Matches(@"^[0-9]{3,30}$");
                v.RuleFor(c => c.Fax).Matches(@"^[0-9]{3,30}$").When(i => !string.IsNullOrWhiteSpace(i.Fax));
            }, companyRegistration);
        }


        private readonly IAccountManagementService _accountManagementService;
        private readonly IAdministratorContext _administratorContext;


        private readonly EdoContext _context;
        private readonly ICustomerContext _customerContext;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IManagementAuditService _managementAuditService;
    }
}