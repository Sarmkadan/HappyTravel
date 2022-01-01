using System.Net;
using System.Threading.Tasks;
using HappyTravel.Edo.Api.Infrastructure;
using HappyTravel.Edo.Api.Models.Branches;
using HappyTravel.Edo.Api.Models.Management;
using HappyTravel.Edo.Api.Services.Customers;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiVersion}/companies")]
    [Produces("application/json")]
    public class CompaniesController : BaseController
    {
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }


        /// <summary>
        ///     Set company verified.
        /// </summary>
        /// <param name="companyId">Id of the company to verify.</param>
        /// <param name="request">Verification details.</param>
        /// <returns></returns>
        [HttpPost("{companyId}/verify")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetCompanyVerified(int companyId, [FromBody] CompanyVerificationRequest request)
        {
            var (isSuccess, _, error) = await _companyService.SetVerified(companyId, request.Reason);

            return isSuccess
                ? (IActionResult) NoContent()
                : BadRequest(ProblemDetailsBuilder.Build(error));
        }


        /// <summary>
        ///     Creates branch for company.
        /// </summary>
        /// <param name="companyId">Company Id.</param>
        /// <param name="branchInfo">Branch information.</param>
        /// <returns></returns>
        [HttpPost("{companyId}/branches")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddBranch(int companyId, [FromBody] BranchInfo branchInfo)
        {
            var (isSuccess, _, _, error) = await _companyService.AddBranch(companyId, branchInfo);

            return isSuccess
                ? (IActionResult) NoContent()
                : BadRequest(ProblemDetailsBuilder.Build(error));
        }


        private readonly ICompanyService _companyService;
    }
}