using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentValidation;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Customers;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        public CustomerService(EdoContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }


        public async Task<Result<Customer>> Add(CustomerRegistrationInfo customerRegistration,
            string externalIdentity,
            string email)
        {
            var (_, isFailure, error) = await Validate(customerRegistration, externalIdentity);
            if (isFailure)
                return Result.Fail<Customer>(error);

            var createdCustomer = new Customer
            {
                Title = customerRegistration.Title,
                FirstName = customerRegistration.FirstName,
                LastName = customerRegistration.LastName,
                Position = customerRegistration.Position,
                Email = email,
                IdentityHash = HashGenerator.ComputeSha256(externalIdentity),
                Created = _dateTimeProvider.UtcNow()
            };

            _context.Customers.Add(createdCustomer);
            await _context.SaveChangesAsync();

            return Result.Ok(createdCustomer);
        }


        public async Task<Result<Customer>> GetMasterCustomer(int companyId)
        {
            var master = await (from c in _context.Customers
                join rel in _context.CustomerCompanyRelations on c.Id equals rel.CustomerId
                where rel.CompanyId == companyId && rel.Type == CustomerCompanyRelationTypes.Master
                select c).FirstOrDefaultAsync();

            if (master is null)
                return Result.Fail<Customer>("Master customer does not exists");

            return Result.Ok(master);
        }


        private async ValueTask<Result> Validate(CustomerRegistrationInfo customerRegistration, string externalIdentity)
        {
            var fieldValidateResult = GenericValidator<CustomerRegistrationInfo>.Validate(v =>
            {
                v.RuleFor(c => c.Title).NotEmpty();
                v.RuleFor(c => c.FirstName).NotEmpty();
                v.RuleFor(c => c.LastName).NotEmpty();
            }, customerRegistration);

            if (fieldValidateResult.IsFailure)
                return fieldValidateResult;

            return await CheckIdentityIsUnique(externalIdentity);
        }


        private async Task<Result> CheckIdentityIsUnique(string identity)
        {
            return await _context.Customers.AnyAsync(c => c.IdentityHash == HashGenerator.ComputeSha256(identity))
                ? Result.Fail("User is already registered")
                : Result.Ok();
        }


        private readonly EdoContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
    }
}