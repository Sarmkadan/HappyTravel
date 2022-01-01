using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Payments;

namespace HappyTravel.Edo.Api.Services.Payments
{
    public interface IPaymentHistoryService
    {
        Task<Result<List<PaymentHistoryData>>> GetCustomerHistory(PaymentHistoryRequest paymentHistoryRequest, int companyId);

        Task<Result<List<PaymentHistoryData>>> GetCompanyHistory(PaymentHistoryRequest paymentHistoryRequest, int companyId);
    }
}