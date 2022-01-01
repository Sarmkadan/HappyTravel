using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure.Options;
using HappyTravel.Edo.Api.Models.Management;
using HappyTravel.Edo.Api.Services.Users;
using HappyTravel.Edo.Common.Enums;
using Microsoft.Extensions.Options;

namespace HappyTravel.Edo.Api.Services.Management
{
    public class AdministratorInvitationService : IAdministratorInvitationService
    {
        public AdministratorInvitationService(IUserInvitationService userInvitationService,
            IOptions<AdministratorInvitationOptions> options,
            IExternalAdminContext externalAdminContext)
        {
            _userInvitationService = userInvitationService;
            _externalAdminContext = externalAdminContext;
            _options = options.Value;
        }


        public Task<Result> SendInvitation(AdministratorInvitationInfo invitationInfo)
        {
            var payload = new GenericInvitationInfo(invitationInfo.Email, invitationInfo.LastName, invitationInfo.FirstName, invitationInfo.Position,
                invitationInfo.Title);

            return _externalAdminContext.IsExternalAdmin()
                ? _userInvitationService.Send(invitationInfo.Email, payload, _options.MailTemplateId, UserInvitationTypes.Administrator)
                : Task.FromResult(Result.Fail("Only external admins can send invitations of this kind."));
        }


        public Task AcceptInvitation(string invitationCode) => _userInvitationService.Accept(invitationCode);


        public Task<Result<AdministratorInvitationInfo>> GetPendingInvitation(string invitationCode)
            => _userInvitationService
                .GetPendingInvitation<AdministratorInvitationInfo>(invitationCode, UserInvitationTypes.Administrator);


        private readonly IExternalAdminContext _externalAdminContext;
        private readonly AdministratorInvitationOptions _options;
        private readonly IUserInvitationService _userInvitationService;
    }
}