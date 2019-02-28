using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Api.Models.Passenger
{
    public class UpdatePassengerRequest
    {
        public int PassengerId { get; set; }

        public PassengerDto Passenger { get; set; }
    }
}
