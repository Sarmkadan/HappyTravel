using System.Threading.Tasks;

namespace HappyTravel.Edo.Api.Infrastructure
{
    public interface ITokenInfoAccessor
    {
        string GetIdentity();

        string GetClientId();

        Task<string> GetAccessToken();
    }
}