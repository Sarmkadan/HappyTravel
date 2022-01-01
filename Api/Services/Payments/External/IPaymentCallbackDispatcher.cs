using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Payments;
using Newtonsoft.Json.Linq;

namespace HappyTravel.Edo.Api.Services.Payments.External
{
    public interface IPaymentCallbackDispatcher
    {
        Task<Result<PaymentResponse>> ProcessCallback(JObject response);
    }
}