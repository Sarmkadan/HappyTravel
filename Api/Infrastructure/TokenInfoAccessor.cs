using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace HappyTravel.Edo.Api.Infrastructure
{
    public class TokenInfoAccessor : ITokenInfoAccessor
    {
        public TokenInfoAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public string GetIdentity() 
            => GetClaimValue("sub");

        public string GetClientId() 
            => GetClaimValue("client_id");

        public async Task<string> GetAccessToken() 
            => await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");


        private string GetClaimValue(string claimType)
            => _httpContextAccessor.HttpContext
                .User
                .Claims
                .SingleOrDefault(c => c.Type == claimType)?.Value;


        private readonly IHttpContextAccessor _httpContextAccessor;
    }
}