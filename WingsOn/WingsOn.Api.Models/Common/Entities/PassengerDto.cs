using System;

namespace WingsOn.Api.Models.Common.Entities
{
    public class PassengerDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateBirth { get; set; }

        public Gender Gender { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
}
