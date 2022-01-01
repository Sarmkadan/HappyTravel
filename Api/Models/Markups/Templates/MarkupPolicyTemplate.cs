using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Markups.Templates
{
    public class MarkupPolicyTemplate
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string[] ParameterNames { get; set; } = Array.Empty<string>();

        [JsonIgnore]
        public bool IsEnabled { get; set; }

        [JsonIgnore]
        public Func<IDictionary<string, decimal>, Func<decimal, decimal>> FunctionFactory { get; set; }

        [JsonIgnore]
        public Func<IDictionary<string, decimal>, bool> SettingsValidator { get; set; }
    }
}