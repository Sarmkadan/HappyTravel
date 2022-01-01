using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Infrastructure.FunctionalExtensions;
using HappyTravel.Edo.Api.Models.Management;
using HappyTravel.Edo.Api.Models.Management.AuditEvents;
using HappyTravel.Edo.Common.Enums;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Management;

namespace HappyTravel.Edo.Api.Services.Management
{
    public class AdministratorRegistrationService : IAdministratorRegistrationService
    {
        public AdministratorRegistrationService(IAdministratorInvitationService invitationService,
            EdoContext context,
            IDateTimeProvider dateTimeProvider,
            IManagementAuditService managementAuditService)
        {
            _invitationService = invitationService;
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _managementAuditService = managementAuditService;
        }


        public async Task<Result> RegisterByInvitation(string invitationCode, string identity)
        {
            return await _invitationService.GetPendingInvitation(invitationCode)
                .OnSuccessWithTransaction(_context, invitation => Result.Ok(invitation)
                    .OnSuccess(CreateAdministrator)
                    .OnSuccess(WriteAuditLog));


            async Task<Administrator> CreateAdministrator(AdministratorInvitationInfo info)
            {
                var now = _dateTimeProvider.UtcNow();
                var administrator = new Administrator
                {
                    Email = info.Email,
                    FirstName = info.FirstName,
                    LastName = info.LastName,
                    IdentityHash = HashGenerator.ComputeSha256(identity),
                    Position = info.Position,
                    Created = now,
                    Updated = now
                };
                _context.Administrators.Add(administrator);
                await _context.SaveChangesAsync();
                return administrator;
            }


            Task<Result> WriteAuditLog(Administrator administrator)
                => _managementAuditService.Write(ManagementEventType.AdministratorRegistration,
                    new AdministrationRegistrationEvent(administrator.Email, administrator.Id, invitationCode));
        }


        private readonly EdoContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly IAdministratorInvitationService _invitationService;
        private readonly IManagementAuditService _managementAuditService;
    }
}