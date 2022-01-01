using System.Globalization;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult OkOrBadRequest<T>(Result<T> model)
        {
            var (_, isFailure, response, error) = model;
            if (isFailure)
                return BadRequest(ProblemDetailsBuilder.Build(error));

            return Ok(response);
        }


        protected async Task<IActionResult> OkOrBadRequest<T>(Task<Result<T>> task) => OkOrBadRequest(await task);


        protected string ClientIp
        {
            get
            {
                var address = HttpContext.Connection.RemoteIpAddress;
                return address.IsIPv4MappedToIPv6
                    ? address.MapToIPv4().ToString()
                    : address.ToString();
            }
        }


        protected string LanguageCode => CultureInfo.CurrentCulture.Name;
    }
}