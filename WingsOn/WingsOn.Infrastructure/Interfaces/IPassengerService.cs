using System.Collections.Generic;
using WingsOn.Api.Models.Passenger;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Infrastructure.Interfaces
{
    public interface IPassengerService
    {
        PassengerDto GetPassengerById(int id);

        List<PassengerDto> GetPassengers(GetPassengersRequest getPassengersRequest);

        List<PassengerDto> GetFlightPassengers(string flightNumber);

        UpdatePassengerResponse UpdatePassenger(UpdatePassengerRequest updatePassengerRequest);
    }
}
