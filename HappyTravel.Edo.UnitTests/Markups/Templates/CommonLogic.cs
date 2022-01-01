using System.Collections.Generic;
using System.Linq;
using HappyTravel.Edo.Api.Services.Markups.Templates;
using Xunit;

namespace HappyTravel.Edo.UnitTests.Markups.Templates
{
    public class CommonLogic
    {
        [Fact]
        public void Should_return_only_available()
        {
            var templates = TemplateService.Get();
            Assert.All(templates, template => Assert.True(template.IsEnabled));
        }

        [Fact]
        public void Invalid_template_id_should_fail_validate()
        {
            var invalidTemplateId = TemplateService.Get().Max(t => t.Id) + 1;
            var validateResult = TemplateService.Validate(invalidTemplateId, new Dictionary<string, decimal>());
            Assert.True(validateResult.IsFailure);
        }
        
        [Fact]
        public void Generic_settings_from_parameter_names_should_be_valid()
        {
            var templates = TemplateService.Get();
            foreach (var template in templates)
            {
                var validSettings = template.ParameterNames
                    .ToDictionary(p => p, p => (decimal)10);
                
                var validateResult = TemplateService.Validate(template.Id, validSettings);
                Assert.True(validateResult.IsSuccess);
            }
        }
        
        private static readonly MarkupPolicyTemplateService TemplateService = new MarkupPolicyTemplateService();
    }
}