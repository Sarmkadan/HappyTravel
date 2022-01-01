using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Edo.Api.Infrastructure
{
    public interface IEntityLocker
    {
        Task<Result> Acquire<TEntity>(string entityId, string locker);

        Task Release<TEntity>(string entityId);
    }
}