using System.Threading.Tasks;
using HappyTravel.Edo.Api.Models.Customers;
using HappyTravel.Edo.Api.Models.Markups;
using HappyTravel.Edo.Common.Enums.Markup;

namespace HappyTravel.Edo.Api.Services.Markups
{
    public interface IMarkupService
    {
        Task<Markup> Get(CustomerInfo customerInfo, MarkupPolicyTarget policyTarget);
    }
}