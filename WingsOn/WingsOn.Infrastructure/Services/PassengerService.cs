using System;
using AutoMapper;
using System.Linq;
using WingsOn.Dal;
using WingsOn.Domain;
using System.Collections.Generic;
using WingsOn.Api.Models.Passenger;
using WingsOn.Infrastructure.Interfaces;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Infrastructure.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IMapper _mapper;

        private readonly PersonRepository _personRepository;
        private readonly BookingRepository _bookingRepository;
        private readonly FlightRepository _flightRepository;


        public PassengerService(IMapper mapper, PersonRepository personRepository, BookingRepository bookingRepository, FlightRepository flightRepository)
        {
            _mapper = mapper;

            _personRepository = personRepository;
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
        }

        public PassengerDto GetPassengerById(int id)
        {
            var passenger = _personRepository.Get(id);
            return _mapper.Map<Person, PassengerDto>(passenger);
        }

        public List<PassengerDto> GetPassengers(GetPassengersRequest getPassengersRequest)
        {
            var passengers = _personRepository.GetAll()?.ToList();

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

        public List<PassengerDto> GetFlightPassengers(string flightNumber)
        {
            var flightPassengers = PrepareFlightPassengers(flightNumber);
            return _mapper.Map<List<Person>, List<PassengerDto>>(flightPassengers);
        }

        public UpdatePassengerResponse UpdatePassenger(UpdatePassengerRequest updatePassengerRequest)
        {
            var response = new UpdatePassengerResponse
            {
                IsSuccessful = true
            };

            try
            {
                var updatedPerson = _mapper.Map<PassengerDto, Person>(updatePassengerRequest.Passenger, opt =>
                    opt.AfterMap((src, dest) => dest.Id = updatePassengerRequest.PassengerId));

                _personRepository.Save(updatedPerson);
            }
            catch (Exception e)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = e.Message;
            }

            return response;
        }

        private List<Person> PrepareFlightPassengers(string flightNumber)
        {
            var flightId = _flightRepository.GetAll()?.FirstOrDefault(flight => flight.Number == flightNumber)?.Id
                ?? throw new Exception("Flight with specified number does not exist.");

            var bookings = _bookingRepository.GetAll()?.Where(booking => booking.Flight.Id == flightId)?.ToList();
            if (bookings == null || !bookings.Any())
            {
                throw new Exception("Booking with specified flight number is not found.");
            }

            return bookings.SelectMany(booking => booking.Passengers)?.ToList();
        }
    }
}
