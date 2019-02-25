using System;
using AutoMapper;
using System.Linq;
using WingsOn.Dal;
using WingsOn.Domain;
using System.Collections.Generic;
using WingsOn.Api.Models.Booking;
using WingsOn.Infrastructure.Interfaces;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;

        private readonly BookingRepository _bookingRepository;
        private readonly PersonRepository _personRepository;
        private readonly FlightRepository _flightRepository;

        public BookingService(IMapper mapper, PersonRepository personRepository, BookingRepository bookingRepository, FlightRepository flightRepository)
        {
            _mapper = mapper;

            _personRepository = personRepository;
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
        }

        public BookingDto GetBookingByNumber(string bookingNumber)
        {
            var bookingByNumber = _bookingRepository.GetAll()?.FirstOrDefault(booking => booking.Number == bookingNumber);
            return _mapper.Map<Booking, BookingDto>(bookingByNumber);
        }

        public CreateBookingResponse CreateBooking(CreateBookingRequest createBookingRequest)
        {
            try
            {
                var booking = PrepareBookingDetails(createBookingRequest);

                _bookingRepository.Save(booking);

                return new CreateBookingResponse
                {
                    IsSuccessful = true,
                    BookingNumber = booking.Number
                };
            }
            catch (Exception e)
            {
                return new CreateBookingResponse
                {
                    IsSuccessful = false,
                    ErrorMessage = e.Message
                };
            }
        }

        private Booking PrepareBookingDetails(CreateBookingRequest createBookingRequest)
        {
            var passengers = createBookingRequest.Passengers
                .Select(passenger => new Person() { Name = passenger.Name, DateBirth = passenger.DateBirth, Gender = (GenderType)passenger.Gender, Address = passenger.Address, Email = passenger.Email })
                .ToList();

            UpdateNewBookingPassengers(passengers);

            var newBookingId = _bookingRepository.GetAll().Max(booking => booking.Id) + 1;
            var flight = _flightRepository.Get(createBookingRequest.FlightId) ?? throw new Exception("Flight with specified ID does not exist."); ;

            return new Booking
            {
                Id = newBookingId,
                Number = GenerateNewBookingNumber(),
                Flight = flight,
                Customer = passengers?.FirstOrDefault(),
                Passengers = passengers,
                DateBooking = DateTime.Now
            };
        }

        private void UpdateNewBookingPassengers(List<Person> passengers)
        {
            var lastAvailableId = _personRepository.GetAll().Max(person => person.Id);
            foreach (var passenger in passengers)
            {
                lastAvailableId++;
                passenger.Id = lastAvailableId;
                _personRepository.Save(passenger);
            }
        }

        private string GenerateNewBookingNumber()
        {
            var rnd = new Random();
            int newBookingNumber = rnd.Next(100000, 900000);
            return $"WO-{newBookingNumber}";
        }
    }
}
