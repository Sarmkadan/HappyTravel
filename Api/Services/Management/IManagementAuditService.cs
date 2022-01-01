using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Api.Services.Management
{
    public interface IManagementAuditService
    {
        Task<Result> Write<TEventData>(ManagementEventType eventType, TEventData eventData);
    }
}