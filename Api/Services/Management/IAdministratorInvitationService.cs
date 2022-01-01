using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Management;

namespace HappyTravel.Edo.Api.Services.Management
{
    public interface IAdministratorInvitationService
    {
        Task<Result> SendInvitation(AdministratorInvitationInfo invitationInfo);

        Task AcceptInvitation(string invitationCode);

        Task<Result<AdministratorInvitationInfo>> GetPendingInvitation(string invitationCode);
    }
}