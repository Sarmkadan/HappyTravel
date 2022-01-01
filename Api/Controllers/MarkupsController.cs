using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Markups;
using HappyTravel.Edo.Api.Models.Markups.Templates;
using HappyTravel.Edo.Api.Services.Markups;
using HappyTravel.Edo.Api.Services.Markups.Templates;
using HappyTravel.Edo.Common.Enums.Markup;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/markups")]
    [Produces("application/json")]
    public class MarkupsController : BaseController
    {
        public MarkupsController(IMarkupPolicyManager policyManager,
            IMarkupPolicyTemplateService policyTemplateService)
        {
            _policyManager = policyManager;
            _policyTemplateService = policyTemplateService;
        }


        /// <summary>
        ///     Creates markup policy.
        /// </summary>
        /// <param name="policyData">Policy data.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> AddPolicy([FromBody] MarkupPolicyData policyData)
        {
            var (_, isFailure, error) = await _policyManager.Add(policyData);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        ///     Deletes policy.
        /// </summary>
        /// <param name="id">Id of the policy to delete.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemovePolicy(int id)
        {
            var (_, isFailure, error) = await _policyManager.Remove(id);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        ///     Updates policy settings.
        /// </summary>
        /// <param name="id">Id of the policy.</param>
        /// <param name="policySettings">Updated settings.</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> ModifyPolicy(int id, [FromBody] MarkupPolicySettings policySettings)
        {
            var (_, isFailure, error) = await _policyManager.Modify(id, policySettings);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return NoContent();
        }


        /// <summary>
        ///     Gets policies for specified scope.
        /// </summary>
        /// <returns>Policies.</returns>
        [HttpGet("{scopeType}/{scopeId}")]
        [ProducesResponseType(typeof(List<MarkupPolicyData>), (int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPolicies(MarkupPolicyScopeType scopeType, int? scopeId)
        {
            var scope = new MarkupPolicyScope(scopeType, scopeId);

            var (_, isFailure, policies, error) = await _policyManager.Get(scope);
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(policies);
        }


        /// <summary>
        ///     Gets policy templates.
        /// </summary>
        /// <returns>Policy templates.</returns>
        [HttpGet("templates")]
        [ProducesResponseType(typeof(ReadOnlyCollection<MarkupPolicyTemplate>), (int) HttpStatusCode.OK)]
        public IActionResult GetPolicyTemplates() => Ok(_policyTemplateService.Get());


        private readonly IMarkupPolicyManager _policyManager;
        private readonly IMarkupPolicyTemplateService _policyTemplateService;
    }
}