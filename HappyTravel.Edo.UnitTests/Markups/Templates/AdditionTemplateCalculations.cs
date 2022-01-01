using System.Collections.Generic;
using HappyTravel.Edo.Api.Services.Markups.Templates;
using Xunit;

namespace HappyTravel.Edo.UnitTests.Markups.Templates
{
    public class AdditionTemplateCalculations
    {
        [Theory]
        [MemberData(nameof(InvalidSettings))]
        public void Invalid_settings_should_fail(Dictionary<string, decimal> settings)
        {
            var validateResult = TemplateService.Validate(AdditionTemplateId, settings);
            Assert.True(validateResult.IsFailure);
        }
        
        [Theory]
        [MemberData(nameof(ValidSettings))]
        public void Valid_settings_should_not_fail(Dictionary<string, decimal> settings)
        {
            var validateResult = TemplateService.Validate(AdditionTemplateId, settings);
            Assert.True(validateResult.IsSuccess);
        }

        [Theory]
        [MemberData(nameof(SettingsAndResults))]
        public void Template_function_should_add(decimal supplierPrice, Dictionary<string, decimal> settings, decimal expectedResultPrice)
        {
            var function = TemplateService.CreateFunction(AdditionTemplateId, settings);
            var resultPrice = function(supplierPrice);
            Assert.Equal(expectedResultPrice, resultPrice);
        }

        public static readonly IEnumerable<object[]> InvalidSettings = new[]
        {
            new object[] {new Dictionary<string, decimal>()},
            new object[] {null},
            new object[] {new Dictionary<string, decimal> {{"fake", 1}}},
            new object[] {new Dictionary<string, decimal> {{"addition", 0}}},
            new object[] {new Dictionary<string, decimal> {{"addition", 1}, {"add", 2}}},
            new object[] {new Dictionary<string, decimal> {{"addition", (decimal)-0.1}}},
            new object[] {new Dictionary<string, decimal> {{"addition", (decimal)-0.99}}}
        };

        public static readonly IEnumerable<object[]> ValidSettings = new[]
        {
            new object[] {new Dictionary<string, decimal> {{"addition", 10}}},
            new object[] {new Dictionary<string, decimal> {{"addition", (decimal) 1.1}}},
            new object[] {new Dictionary<string, decimal> {{"addition", 99}}},
        };
        
        public static readonly IEnumerable<object[]> SettingsAndResults = new[]
        {
            new object[] {100, new Dictionary<string, decimal> {{"addition", 10}}, 110},
            new object[] {20, new Dictionary<string, decimal> {{"addition", (decimal) 1.1}}, 21.1},
            new object[] {3, new Dictionary<string, decimal> {{"addition", 99}}, 102},
            new object[] {45.5, new Dictionary<string, decimal> {{"addition", (decimal)2.18}}, 47.68}
        };

        private const int AdditionTemplateId = 2;
        
        private static readonly MarkupPolicyTemplateService TemplateService = new MarkupPolicyTemplateService();
    }
}