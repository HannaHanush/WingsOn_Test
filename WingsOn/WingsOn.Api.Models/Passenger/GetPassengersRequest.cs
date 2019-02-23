using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Api.Models.Passenger
{
    public class GetPassengersRequest
    {
        public string FlightNumber { get; set; }

        public Gender? Gender { get; set; }
    }
}
