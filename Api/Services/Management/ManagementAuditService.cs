using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Management;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Services.Management
{
    public class ManagementAuditService : IManagementAuditService
    {
        public ManagementAuditService(EdoContext context,
            IDateTimeProvider dateTimeProvider,
            IAdministratorContext administratorContext)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _administratorContext = administratorContext;
        }


        public async Task<Result> Write<TEventData>(ManagementEventType eventType, TEventData eventData)
        {
            var (_, isFailure, admin, error) = await _administratorContext.GetCurrent();
            if (isFailure)
                return Result.Fail(error);

            var logEntry = new ManagementAuditLogEntry
            {
                Created = _dateTimeProvider.UtcNow(),
                AdministratorId = admin.Id,
                Type = eventType,
                EventData = JsonConvert.SerializeObject(eventData)
            };

            _context.ManagementAuditLog.Add(logEntry);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }


        private readonly IAdministratorContext _administratorContext;

        private readonly EdoContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
    }
}