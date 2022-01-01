using System.Collections.Generic;
using HappyTravel.Edo.Data.Markup;

namespace HappyTravel.Edo.Api.Models.Markups
{
    public class Markup
    {
        public AggregatedMarkupFunction Function { get; set; }

        public List<MarkupPolicy> Policies { get; set; }
    }
}