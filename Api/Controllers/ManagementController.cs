using System.Net;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Management;
using HappyTravel.Edo.Api.Services.Management;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/management")]
    [Produces("application/json")]
    public class ManagementController : BaseController
    {
        public ManagementController(IAdministratorInvitationService invitationService,
            IAdministratorRegistrationService registrationService,
            ITokenInfoAccessor tokenInfoAccessor)
        {
            _invitationService = invitationService;
            _registrationService = registrationService;
            _tokenInfoAccessor = tokenInfoAccessor;
        }


        /// <summary>
        ///     Send invitation to administrator.
        /// </summary>
        /// <param name="invitationInfo">Administrator invitation info.</param>
        /// <returns></returns>
        [HttpPost("admin/invite")]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> InviteAdministrator([FromBody] AdministratorInvitationInfo invitationInfo)
        {
            var (_, isFailure, error) = await _invitationService.SendInvitation(invitationInfo);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        ///     Register current user as administrator by invitation code.
        /// </summary>
        /// <param name="invitationCode">Invitation code.</param>
        /// <returns></returns>
        [HttpPost("admin/register")]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> RegisterAdministrator(string invitationCode)
        {
            var identity = _tokenInfoAccessor.GetIdentity();
            if (string.IsNullOrWhiteSpace(identity))
                return BadRequest(ProblemDetailsBuilder.Build("Could not get user identity"));

            var (_, isFailure, error) = await _registrationService.RegisterByInvitation(invitationCode, identity);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        private readonly IAdministratorInvitationService _invitationService;
        private readonly IAdministratorRegistrationService _registrationService;
        private readonly ITokenInfoAccessor _tokenInfoAccessor;
    }
}