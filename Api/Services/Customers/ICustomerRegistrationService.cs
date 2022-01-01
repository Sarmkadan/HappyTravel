using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Customers;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public interface ICustomerRegistrationService
    {
        Task<Result> RegisterWithCompany(CustomerRegistrationInfo customerData, CompanyRegistrationInfo companyData,
            string externalIdentity, string email);


        Task<Result> RegisterInvited(CustomerRegistrationInfo registrationInfo,
            string invitationCode, string externalIdentity, string email);
    }
}