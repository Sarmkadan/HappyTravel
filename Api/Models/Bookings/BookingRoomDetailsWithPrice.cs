using Newtonsoft.Json;

namespace HappyTravel.Edo.Api.Models.Bookings
{
    public struct BookingRoomDetailsWithPrice
    {
        [JsonConstructor]
        public BookingRoomDetailsWithPrice(BookingRoomDetails roomDetails, BookingRoomPrice price)
        {
            RoomDetails = roomDetails;
            Price = price;
        }


        public BookingRoomDetails RoomDetails { get; }
        public BookingRoomPrice Price { get; }
    }
}