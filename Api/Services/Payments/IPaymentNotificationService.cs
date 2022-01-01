using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Payments;

namespace HappyTravel.Edo.Api.Services.Payments
{
    public interface IPaymentNotificationService
    {
        Task<Result> SendBillToCustomer(PaymentBill paymentBill);

        Task<Result> SendNeedPaymentNotificationToCustomer(PaymentBill paymentBill);
    }
}