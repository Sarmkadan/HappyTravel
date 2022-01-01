using HappyTravel.Edo.Common.Enums.Markup;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Markups
{
    public readonly struct MarkupPolicyData
    {
        [JsonConstructor]
        public MarkupPolicyData(MarkupPolicyTarget target, MarkupPolicySettings settings,
            MarkupPolicyScope scope)
        {
            Target = target;
            Settings = settings;
            Scope = scope;
        }


        /// <summary>
        ///     Policy target.
        /// </summary>
        public MarkupPolicyTarget Target { get; }

        /// <summary>
        ///     Policy settings.
        /// </summary>
        public MarkupPolicySettings Settings { get; }

        /// <summary>
        ///     Policy scopes.
        /// </summary>
        public MarkupPolicyScope Scope { get; }
    }
}