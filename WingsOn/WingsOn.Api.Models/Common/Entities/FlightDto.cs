using System;

namespace WingsOn.Api.Models.Common.Entities
{
    public class FlightDto
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public AirlineDto Carrier { get; set; }

        public AirportDto DepartureAirport { get; set; }

        public DateTime DepartureDate { get; set; }

        public AirportDto ArrivalAirport { get; set; }

        public DateTime ArrivalDate { get; set; }

        public decimal Price { get; set; }
    }
}
