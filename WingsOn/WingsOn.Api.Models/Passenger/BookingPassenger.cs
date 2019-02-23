using System;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Api.Models.Passenger
{
    public class BookingPassenger
    {
        public string Name { get; set; }

        public DateTime DateBirth { get; set; }

        public Gender Gender { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
}
