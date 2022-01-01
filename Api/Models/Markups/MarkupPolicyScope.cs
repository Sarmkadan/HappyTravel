using HappyTravel.Edo.Common.Enums.Markup;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Markups
{
    public readonly struct MarkupPolicyScope
    {
        [JsonConstructor]
        public MarkupPolicyScope(MarkupPolicyScopeType type, int? scopeId = null)
        {
            Type = type;
            ScopeId = scopeId;
        }


        public void Deconstruct(out MarkupPolicyScopeType type, out int? companyId, out int? branchId, out int? customerId)
        {
            type = Type;
            companyId = null;
            branchId = null;
            customerId = null;

            switch (type)
            {
                case MarkupPolicyScopeType.Company:
                    companyId = ScopeId;
                    break;
                case MarkupPolicyScopeType.Branch:
                    branchId = ScopeId;
                    break;
                case MarkupPolicyScopeType.Customer:
                    customerId = ScopeId;
                    break;
            }
        }


        /// <summary>
        ///     Scope type.
        /// </summary>
        public MarkupPolicyScopeType Type { get; }

        /// <summary>
        ///     Scope entity Id, can be null for global policies.
        /// </summary>
        public int? ScopeId { get; }
    }
}