using System.Threading.Tasks;
using HappyTravel.Edo.Data;
using HappyTravel.Edo.Data.Booking;
using HappyTravel.EdoContracts.Accommodations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Services.Accommodations
{
    public class BookingAuditLogService: IBookingAuditLogService
    {
        public BookingAuditLogService(EdoContext edoContext)
        {
            _edoContext = edoContext;
        }
        
        /// <summary>
        /// Adds new booking details data to the log table
        /// </summary>
        /// <param name="newBookingDetails">Booking details that will be written to the booking table</param>
        /// <param name="currentBookingData">Current booking data from the booking table where BookingDetails value will be rewritten by the newBookingDetails value</param>
        /// <returns></returns>
        public async Task Add(BookingDetails newBookingDetails, Booking currentBookingData = null)
        {
            if (currentBookingData is null)
                currentBookingData = await _edoContext.Bookings.SingleOrDefaultAsync(i => i.ReferenceCode.Equals(newBookingDetails.ReferenceCode));

            await Add(currentBookingData.Id,
                currentBookingData.CustomerId,
                JsonConvert.DeserializeObject<BookingDetails>(currentBookingData.BookingDetails),
                newBookingDetails);
        }


        private Task Add(int bookingId, int customerId,  BookingDetails currentBookingDetails, BookingDetails newBookingDetails)
        {
            _edoContext.BookingAuditLog.Add(new BookingAuditLogEntry
            {
                BookingId = bookingId,
                CustomerId = customerId,
                BookingDetails = newBookingDetails,
                PreviousBookingDetails = currentBookingDetails
            });
            
            return _edoContext.SaveChangesAsync();
        }
        
        
        private readonly EdoContext _edoContext;
    }
}