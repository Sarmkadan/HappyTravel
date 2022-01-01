using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Markups;

namespace HappyTravel.Edo.Api.Services.Markups
{
    public interface IMarkupPolicyManager
    {
        Task<Result> Add(MarkupPolicyData policyData);

        Task<Result> Remove(int policyId);

        Task<Result> Modify(int policyId, MarkupPolicySettings settings);

        Task<Result<List<MarkupPolicyData>>> Get(MarkupPolicyScope scope);
    }
}