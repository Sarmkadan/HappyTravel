using System;
using System.Collections.Generic;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Data.Markup
{
    public class AppliedMarkup
    {
        public int Id { get; set; }
        public ServiceTypes ServiceType { get; set; }
        public List<MarkupPolicy> Policies { get; set; }
        public string ReferenceCode { get; set; }
        public DateTime Created { get; set; }
    }
}