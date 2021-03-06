using System.Net;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Infrastructure
{
    public static class ProblemDetailsBuilder
    {
        public static ProblemDetails Build(string details, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new ProblemDetails
            {
                Detail = details,
                Status = (int) statusCode
            };


        public static Result<T, ProblemDetails> Fail<T>(string details, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => Result.Fail<T, ProblemDetails>(Build(details, statusCode));
    }
}