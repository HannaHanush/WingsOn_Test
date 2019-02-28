using System.Collections.Generic;
using WingsOn.Api.Models.Passenger;
using Microsoft.AspNetCore.JsonPatch;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Infrastructure.Interfaces
{
    public interface IPassengerService
    {
        PassengerDto GetPassengerById(int id);

        List<PassengerDto> GetPassengers(GetPassengersRequest getPassengersRequest);

        void UpdatePassenger(int passengerId, JsonPatchDocument<PassengerDto> passengerPatch);
    }
}
