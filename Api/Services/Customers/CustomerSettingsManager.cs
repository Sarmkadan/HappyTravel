using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure.Converters;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Customers;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Edo.Api.Services.Customers
{
    public class CustomerSettingsManager : ICustomerSettingsManager
    {
        public CustomerSettingsManager(EdoContext context, IJsonSerializer serializer)
        {
            _context = context;
            _serializer = serializer;
        }


        public async Task<Result> SetAppSettings(CustomerInfo customerInfo, string appSettings)
        {
            var (_, isFailure, customer, error) = await GetCustomer(customerInfo);
            if (isFailure)
                return Result.Fail(error);

            customer.AppSettings = appSettings;
            _context.Update(customer);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }


        public async Task<Result<string>> GetAppSettings(CustomerInfo customerInfo)
        {
            var settings = await _context.Customers
                .Where(c => c.Id == customerInfo.CustomerId)
                .Select(c => c.AppSettings)
                .SingleOrDefaultAsync();

            return settings == default
                ? Result.Fail<string>("Could not find the customer")
                : Result.Ok(settings);
        }


        public async Task<Result> SetUserSettings(CustomerInfo customerInfo, CustomerUserSettings userSettings)
        {
            var (_, isFailure, customer, error) = await GetCustomer(customerInfo);
            if (isFailure)
                return Result.Fail(error);

            customer.UserSettings = _serializer.SerializeObject(userSettings);
            _context.Update(customer);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }


        public async Task<Result<CustomerUserSettings>> GetUserSettings(CustomerInfo customerInfo)
        {
            var settings = await _context.Customers
                .Where(c => c.Id == customerInfo.CustomerId)
                .Select(c => c.UserSettings)
                .SingleOrDefaultAsync();

            return settings == default
                ? Result.Fail<CustomerUserSettings>("Could not find the customer")
                : Result.Ok(_serializer.DeserializeObject<CustomerUserSettings>(settings));
        }


        private async Task<Result<Customer>> GetCustomer(CustomerInfo customerInfo)
        {
            var customer = await _context.Customers
                .SingleOrDefaultAsync(c => c.Id == customerInfo.CustomerId);

            return customer == default
                ? Result.Fail<Customer>("Could not find the customer")
                : Result.Ok(customer);
        }


        private readonly EdoContext _context;
        private readonly IJsonSerializer _serializer;
    }
}