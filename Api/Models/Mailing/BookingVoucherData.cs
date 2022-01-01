using System.Collections.Generic;
using HappyTravel.Edo.Api.Models.Bookings;

namespace HappyTravel.Edo.Api.Models.Mailing
{
    public class BookingVoucherData
    {
        public string BookingId { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public List<BookingRoomDetails> RoomDetails { get; set; }
        public string AccommodationName { get; set; }
        public string ReferenceCode { get; set; }
    }
}