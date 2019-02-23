using System;
using System.Collections.Generic;
using WingsOn.Api.Models.Passenger;

namespace WingsOn.Api.Models.Booking
{
    public class CreateBookingRequest
    {
        public int FlightId { get; set; }

        public List<BookingPassenger> Passengers { get; set; }
    }
}
