using System.Collections.Generic;
using System.Threading.Tasks;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data.Markup;

namespace HappyTravel.Edo.Api.Services.Markups
{
    public interface IMarkupLogger
    {
        Task Write(string referenceCode, ServiceTypes serviceType, List<MarkupPolicy> policies);
    }
}