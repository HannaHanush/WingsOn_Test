using WingsOn.Api.Models.Booking;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Infrastructure.Interfaces
{
    public interface IBookingService
    {
        BookingDto GetBookingByNumber(string bookingNumber);

        CreateBookingResponse CreateBooking(CreateBookingRequest createBookingRequest);
    }
}
