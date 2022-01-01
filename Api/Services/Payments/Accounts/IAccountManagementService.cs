using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Data.Customers;
using HappyTravel.Edo.Data.Payments;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.Edo.Api.Services.Payments.Accounts
{
    public interface IAccountManagementService
    {
        Task<Result> Create(Company company, Currencies currency);

        Task<Result> ChangeCreditLimit(int accountId, decimal creditLimit);

        Task<Result<PaymentAccount>> Get(int companyId, Currencies currency);
    }
}