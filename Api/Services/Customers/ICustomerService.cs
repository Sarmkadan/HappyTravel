using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Data.Customers;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public interface ICustomerService
    {
        Task<Result<Customer>> Add(CustomerRegistrationInfo customerRegistration, string externalIdentity, string email);

        Task<Result<Customer>> GetMasterCustomer(int companyId);
    }
}