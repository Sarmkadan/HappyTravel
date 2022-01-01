using HappyTravel.Edo.Api.Infrastructure;

namespace HappyTravel.Edo.Api.Services.Management
{
    public class ExternalAdminContext : IExternalAdminContext
    {
        public ExternalAdminContext(ITokenInfoAccessor tokenInfoAccessor)
        {
            _tokenInfoAccessor = tokenInfoAccessor;
        }


        public bool IsExternalAdmin() => _tokenInfoAccessor.GetClientId() == ExternalAdminClientName;

        private const string ExternalAdminClientName = "externalAdmin";
        private readonly ITokenInfoAccessor _tokenInfoAccessor;
    }
}