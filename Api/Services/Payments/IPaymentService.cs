using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Bookings;
using HappyTravel.Edo.Data.Booking;
using HappyTravel.EdoContracts.General;
using HappyTravel.EdoContracts.General.Enums;

namespace HappyTravel.Edo.Api.Services.Payments
{
    public interface IPaymentService
    {
        IReadOnlyCollection<Currencies> GetCurrencies();

        IReadOnlyCollection<PaymentMethods> GetAvailableCustomerPaymentMethods();

        Task<Result<List<int>>> GetBookingsForCapture(DateTime deadlineDate);

        Task<Result<ProcessResult>> CaptureMoneyForBookings(List<int> bookingIds);

        Task<Result> VoidMoney(Booking booking);

        Task<Result> CompleteOffline(int bookingId);

        Task<Result<ProcessResult>> NotifyPaymentsNeeded(List<int> bookingIds);
        Task<Result<Price>> GetPendingAmount(int bookingId);
    }
}