using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Management.Enums;
using HappyTravel.Edo.Api.Models.Users;
using HappyTravel.Edo.Data.Management;

namespace HappyTravel.Edo.Api.Services.Management
{
    public interface IAdministratorContext
    {
        Task<bool> HasPermission(AdministratorPermissions permission);

        Task<Result<Administrator>> GetCurrent();

        Task<Result<UserInfo>> GetUserInfo();
    }
}