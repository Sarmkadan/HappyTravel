using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Api.Models.Users;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public interface ICustomerContext
    {
        ValueTask<Result<CustomerInfo>> GetCustomerInfo();

        Task<Result<UserInfo>> GetUserInfo();

        Task<List<CustomerCompanyInfo>> GetCustomerCompanies();
        
        ValueTask<Result<CustomerInfo>> SetCustomerInfo(int customerId);
    }
}