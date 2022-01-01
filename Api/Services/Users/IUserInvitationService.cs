using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Management;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Api.Services.Users
{
    public interface IUserInvitationService
    {
        Task<Result> Send(string email, GenericInvitationInfo invitationInfo, string mailTemplateId, UserInvitationTypes invitationType);

        Task Accept(string invitationCode);

        Task<Result<TInvitationData>> GetPendingInvitation<TInvitationData>(string invitationCode, UserInvitationTypes invitationType);
    }
}