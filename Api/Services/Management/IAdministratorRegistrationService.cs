using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace HappyTravel.Edo.Api.Services.Management
{
    public interface IAdministratorRegistrationService
    {
        Task<Result> RegisterByInvitation(string invitationCode, string identity);
    }
}