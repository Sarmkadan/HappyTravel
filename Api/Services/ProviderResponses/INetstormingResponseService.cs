using System.IO;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Services.ProviderResponses
{
    public interface INetstormingResponseService
    {
        Task<Result> ProcessBookingDetailsResponse(byte[] xmlRequestData);
    }
}