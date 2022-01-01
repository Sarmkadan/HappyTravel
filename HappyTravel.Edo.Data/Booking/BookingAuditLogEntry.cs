using System;
using HappyTravel.EdoContracts.Accommodations;

namespace HappyTravel.Edo.Data.Booking
{
    public class BookingAuditLogEntry
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public BookingDetails  PreviousBookingDetails {get; set; }
        public BookingDetails  BookingDetails {get; set; }
    }
}