using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Markups.Templates;

namespace HappyTravel.Edo.Api.Services.Markups.Templates
{
    public class MarkupPolicyTemplateService : IMarkupPolicyTemplateService
    {
        public IReadOnlyCollection<MarkupPolicyTemplate> Get() => new ReadOnlyCollection<MarkupPolicyTemplate>(Templates);


        public Func<decimal, decimal> CreateFunction(int templateId, IDictionary<string, decimal> settings)
        {
            var template = Templates.Single(t => t.Id == templateId);
            var (_, isFailure, error) = Validate(template, settings);
            // This is not normal case but it would be better to double check this to avoid errors.
            if (isFailure)
                throw new Exception(error);

            return template.FunctionFactory(settings);
        }


        public Result Validate(int templateId, IDictionary<string, decimal> settings)
        {
            if (settings is null)
                return Result.Fail<MarkupPolicyTemplate>("Invalid settings");

            var template = Templates.SingleOrDefault(t => t.Id == templateId);
            if (template == default)
                return Result.Fail<MarkupPolicyTemplate>($"Could not find template by id {templateId}");

            return Validate(template, settings);
        }


        private Result Validate(MarkupPolicyTemplate template, IDictionary<string, decimal> settings)
        {
            if (!template.IsEnabled)
                return Result.Fail<MarkupPolicyTemplate>("Could not create expression for disabled template");

            if (!template.SettingsValidator(settings))
                return Result.Fail<MarkupPolicyTemplate>("Invalid template settings");

            return Result.Ok();
        }


        private const string MultiplyingFactorSetting = "factor";
        private const string AdditionValueSetting = "addition";

        // !! These templates are referenced from MarkupPolicies and should not be changed without appropriate migration.
        private static readonly MarkupPolicyTemplate[] Templates =
        {
            new MarkupPolicyTemplate
            {
                Id = 1,
                Title = "Multiplication",
                ParameterNames = new[] {MultiplyingFactorSetting},
                IsEnabled = true,
                FunctionFactory = settings => rawValue => rawValue * settings[MultiplyingFactorSetting],
                SettingsValidator = settings => settings.Keys.Count == 1 &&
                    settings.ContainsKey(MultiplyingFactorSetting) &&
                    settings[MultiplyingFactorSetting] > 1
            },
            new MarkupPolicyTemplate
            {
                Id = 2,
                Title = "Addition",
                ParameterNames = new[] {AdditionValueSetting},
                IsEnabled = true,
                FunctionFactory = settings => rawValue => rawValue + settings[AdditionValueSetting],
                SettingsValidator = settings => settings.Keys.Count == 1 &&
                    settings.ContainsKey(AdditionValueSetting) &&
                    settings[AdditionValueSetting] > 0
            }
        };
    }
}