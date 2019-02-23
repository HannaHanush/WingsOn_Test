using System;
using System.Collections.Generic;

namespace WingsOn.Api.Models.Common.Entities
{
    public class BookingDto
    {
        public string Number { get; set; }

        public FlightDto Flight { get; set; }

        public PassengerDto Customer { get; set; }

        public List<PassengerDto> Passengers { get; set; }

        public DateTime DateBooking { get; set; }
    }
}
