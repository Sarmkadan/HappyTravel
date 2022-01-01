using System.Collections.Generic;
using HappyTravel.Edo.Api.Models.Bookings;

namespace HappyTravel.Edo.Api.Models.Mailing
{
    public class BookingInvoiceData
    {
        public List<BookingRoomDetailsWithPrice> RoomDetails { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string AccommodationName { get; set; }
        public string CurrencyCode { get; set; }
        public string PriceTotal { get; set; }
    }
}