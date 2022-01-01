using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Mailing;

namespace HappyTravel.Edo.Api.Services.Accommodations
{
    public interface IBookingDocumentsService
    {
        Task<Result<BookingVoucherData>> GenerateVoucher(int bookingId);

        Task<Result<BookingInvoiceData>> GenerateInvoice(int bookingId);
    }
}