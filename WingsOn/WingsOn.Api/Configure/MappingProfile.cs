using AutoMapper;
using WingsOn.Domain;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Api.Configure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GenderType, Gender>();
            CreateMap<Person, PassengerDto>();
            CreateMap<Booking, BookingDto>();
            CreateMap<Flight, FlightDto>();
            CreateMap<PassengerDto, Person>();
            CreateMap<Gender, GenderType>();
        }
    }
}
