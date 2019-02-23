namespace WingsOn.Api.Models.Booking
{
    public class CreateBookingResponse
    {
        public bool IsSuccessful { get; set; }

        public string BookingNumber { get; set; }

        public string ErrorMessage { get; set; }
    }
}
