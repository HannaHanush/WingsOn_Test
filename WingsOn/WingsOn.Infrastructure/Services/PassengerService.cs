using AutoMapper;
using System.Linq;
using WingsOn.Dal;
using WingsOn.Domain;
using System.Collections.Generic;
using WingsOn.Api.Models.Passenger;
using WingsOn.Api.Models.Exceptions;
using WingsOn.Infrastructure.Interfaces;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Infrastructure.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IMapper _mapper;

        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IRepository<Flight> _flightRepository;


        public PassengerService(IMapper mapper, IRepository<Person> personRepository, IRepository<Booking> bookingRepository, IRepository<Flight> flightRepository)
        {
            _mapper = mapper;

            _personRepository = personRepository;
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
        }

        public PassengerDto GetPassengerById(int id)
        {
            var passenger = _personRepository.Get(id) ?? throw new WingsOnNotFoundException("Passenger with specified ID is not found.");
            return _mapper.Map<Person, PassengerDto>(passenger);
        }

        public List<PassengerDto> GetPassengers(GetPassengersRequest getPassengersRequest)
        {
            var passengers = _personRepository.GetAll()?.ToList();
            if (passengers == null || !passengers.Any())
            {
                throw new WingsOnNotFoundException("Passengers are not found.");
            }

            if (getPassengersRequest.Gender != null)
            {
                passengers = passengers?.Where(person => person.Gender == (GenderType)getPassengersRequest.Gender)?.ToList();
            }

            if (getPassengersRequest.FlightNumber != null)
            {
                var flightPassengersIds = PrepareFlightPassengers(getPassengersRequest.FlightNumber)
                    ?.Select(person => person.Id).ToList();

                passengers = passengers?.Where(person => flightPassengersIds.Contains(person.Id))?.ToList();
            }
            
            return _mapper.Map<List<Person>, List<PassengerDto>>(passengers);
        }

        public void UpdatePassenger(UpdatePassengerRequest updatePassengerRequest)
        {
            var passenger = GetPassengerById(updatePassengerRequest.PassengerId);

            updatePassengerRequest.PassengerPatch.ApplyTo(passenger);

            var updatedPerson = _mapper.Map<PassengerDto, Person>(passenger, opt =>
                    opt.AfterMap((src, dest) => dest.Id = updatePassengerRequest.PassengerId));

            _personRepository.Save(updatedPerson);
        }

        private List<Person> PrepareFlightPassengers(string flightNumber)
        {
            var flightId = _flightRepository.GetAll()?.FirstOrDefault(flight => flight.Number == flightNumber)?.Id
                ?? throw new WingsOnNotFoundException("Flight with specified number does not exist.");

            var bookings = _bookingRepository.GetAll()?.Where(booking => booking.Flight.Id == flightId)?.ToList();
            if (bookings == null || !bookings.Any())
            {
                throw new WingsOnNotFoundException("Booking with specified flight number is not found.");
            }

            return bookings.SelectMany(booking => booking.Passengers)?.ToList();
        }
    }
}
