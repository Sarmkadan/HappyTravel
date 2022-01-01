using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Markups.Templates;

namespace HappyTravel.Edo.Api.Services.Markups.Templates
{
    public interface IMarkupPolicyTemplateService
    {
        IReadOnlyCollection<MarkupPolicyTemplate> Get();

        Func<decimal, decimal> CreateFunction(int templateId, IDictionary<string, decimal> settings);

        Result Validate(int templateId, IDictionary<string, decimal> settings);
    }
}