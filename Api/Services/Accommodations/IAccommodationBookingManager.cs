using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using HappyTravel.Edo.Api.Models.Accommodations;
using HappyTravel.Edo.Api.Models.Bookings;
using HappyTravel.Edo.Data.Booking;
using HappyTravel.EdoContracts.Accommodations;
using HappyTravel.EdoContracts.General.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Edo.Api.Services.Accommodations
{
    public interface IAccommodationBookingManager
    {
        Task<Result<BookingDetails, ProblemDetails>> Book(AccommodationBookingRequest bookingRequest, Booking booking, string languageCode);
        
        Task<Result<AccommodationBookingInfo>> GetCustomerBookingInfo(int bookingId);
        
        Task<Result<AccommodationBookingInfo>> GetCustomerBookingInfo(string referenceCode);
        
        Task<Result<Booking>> Get(string referenceCode);
        
        Task<Result<Booking>> Get(int id);
        
        Task<Result<List<SlimAccommodationBookingInfo>>> GetCustomerBookingsInfo();
        
        Task<Result<Booking, ProblemDetails>> CancelBooking(int bookingId);
        
        Task<Result> ConfirmBooking(BookingDetails bookingDetails, Booking booking);
        
        Task<Result> ConfirmBookingCancellation(BookingDetails bookingDetails, Booking booking);
        
        Task<Result> UpdateBookingDetails(BookingDetails bookingDetails, Booking booking);
 
        Task<Result<string>> CreateForPayment(PaymentMethods paymentMethod, string itineraryNumber, BookingAvailabilityInfo bookingAvailability,
            string countryCode);
    }
}