using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Infrastructure.Converters;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Api.Services.Customers;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Customers;
using HappyTravel.Edo.UnitTests.Infrastructure;
using HappyTravel.Edo.UnitTests.Infrastructure.DbSetMocks;
using HappyTravel.EdoContracts.General.Enums;
using Moq;
using Xunit;

namespace HappyTravel.Edo.UnitTests.Customers.CustomerSettings
{
    public class UserSettings
    {
        public UserSettings(Mock<EdoContext> edoContextMock, IJsonSerializer serializer)
        {
            _settingsManager = new CustomerSettingsManager(edoContextMock.Object, serializer);
            edoContextMock
                .Setup(c => c.Customers)
                .Returns(DbSetMockProvider.GetDbSetMock(new List<Customer>
                {
                    new Customer
                    {
                        Id = 1,
                    }
                }));
        }
        
        [Theory]
        [MemberData(nameof(SettingsList))]
        public async Task Stored_settings_should_be_retrieved(CustomerUserSettings settings)
        {
            var customer = CustomerInfoFactory.GetByCustomerId(1);
            await _settingsManager.SetUserSettings(customer, settings);
            var (_, _, storedSettings, _) = await _settingsManager.GetUserSettings(customer);
            
            Assert.Equal(settings, storedSettings);
        }
        
        [Fact]
        public async Task Invalid_customer_should_fail_set_settings()
        {
            var customer = CustomerInfoFactory.GetByCustomerId(200);
            var (_, isFailure, _) = await _settingsManager.SetUserSettings(customer, It.IsAny<CustomerUserSettings>());
            Assert.True(isFailure);
        }
        
        [Fact]
        public async Task Invalid_customer_should_fail_get_settings()
        {
            var customer = CustomerInfoFactory.GetByCustomerId(200);
            var (_, isFailure, _) = await _settingsManager.GetUserSettings(customer);
            Assert.True(isFailure);
        }
        
        private readonly CustomerSettingsManager _settingsManager;

        public static readonly IEnumerable<object[]> SettingsList = new[]
        {
            new object[] {new CustomerUserSettings(true, Currencies.EUR)},
            new object[] {new CustomerUserSettings(false, Currencies.USD)},
            new object[] {new CustomerUserSettings(false, Currencies.EUR)},
            new object[] {new CustomerUserSettings(true, Currencies.USD)},
            new object[] {default(CustomerUserSettings)}
        };
    }
}