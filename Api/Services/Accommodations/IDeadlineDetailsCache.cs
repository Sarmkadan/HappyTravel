using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Edo.Api.Services.Accommodations
{
    public interface IDeadlineDetailsCache
    {
        Result<DeadlineDetails> Get(string agreementId);

        void Set(string agreementId, DeadlineDetails deadlineDetails);
    }
}