using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Users;
using HappyTravel.Edo.Data.Management;

namespace HappyTravel.Edo.Api.Services.Management
{
    public interface IServiceAccountContext
    {
        Task<Result<ServiceAccount>> GetCurrent();

        Task<Result<UserInfo>> GetUserInfo();
    }
}