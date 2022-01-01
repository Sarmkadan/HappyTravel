using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Customers;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public interface ICustomerSettingsManager
    {
        Task<Result> SetAppSettings(CustomerInfo customerInfo, string appSettings);

        Task<Result<string>> GetAppSettings(CustomerInfo customerInfo);

        Task<Result> SetUserSettings(CustomerInfo customerInfo, CustomerUserSettings userSettings);

        Task<Result<CustomerUserSettings>> GetUserSettings(CustomerInfo customerInfo);
    }
}