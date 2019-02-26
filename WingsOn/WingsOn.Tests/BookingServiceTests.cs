using Moq;
using System;
using AutoMapper;
using System.Linq;
using WingsOn.Dal;
using WingsOn.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using WingsOn.Infrastructure.Services;
using WingsOn.Api.Models.Common.Entities;
using WingsOn.Api.Models.Booking;
using WingsOn.Api.Models.Passenger;

namespace WingsOn.Tests
{
    public class BookingServiceTests
    {
        [Test]
        public void Get_WhenCorrectData_ShouldGetProperly()
        {
            // Arrange 
            var flight = PrepareFlightDetails();
            var person = new Person { Id = 1, Name = "Test", DateBirth = DateTime.Now.Date, Gender = GenderType.Male, Address = "TestAddress", Email = "email@email.com" };

            var testBooking = new Booking { Id = 77, Number = "54321", Flight = flight, Customer = person, DateBooking = DateTime.Now, Passengers = new List<Person> { person } };
            var bookings = new List<Booking>
            {
                testBooking,
                new Booking { Id = 87, Number = "12345", Flight = flight, Customer = person, DateBooking = DateTime.Now, Passengers = new List<Person> { person } }
            };

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Booking, BookingDto>());

            var mockPersonRepository = new Mock<IRepository<Person>>();
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockFlightRepository = new Mock<IRepository<Flight>>();

            mockBookingRepository.Setup(x => x.GetAll()).Returns(bookings);

            var bookingService = new BookingService(Mapper.Instance, mockPersonRepository.Object, mockBookingRepository.Object, mockFlightRepository.Object);

            // Act 
            var expectedBooking = bookingService.GetBookingByNumber(testBooking.Number);

            // Assert
            Assert.AreEqual(expectedBooking.Number, testBooking.Number);
            Assert.AreEqual(expectedBooking.Flight.Number, testBooking.Flight.Number);
            Assert.AreEqual(expectedBooking.Customer.Name, testBooking.Customer.Name);
            Assert.AreEqual(expectedBooking.Passengers.Count, testBooking.Passengers.ToList().Count);
            Assert.AreEqual(expectedBooking.DateBooking.Date, testBooking.DateBooking.Date);
        }

        [Test]
        public void Get_WhenIncorrectData_ShouldReturnNull()
        {
            // Arrange 
            var flight = PrepareFlightDetails();
            var person = new Person { Id = 1, Name = "Test", DateBirth = DateTime.Now.Date, Gender = GenderType.Male, Address = "TestAddress", Email = "email@email.com" };

            var testBooking = new Booking { Id = 77, Number = "54321", Flight = flight, Customer = person, DateBooking = DateTime.Now, Passengers = new List<Person> { person } };
            var bookings = new List<Booking>
            {
                testBooking,
                new Booking { Id = 87, Number = "12345", Flight = flight, Customer = person, DateBooking = DateTime.Now, Passengers = new List<Person> { person } }
            };

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Booking, BookingDto>());

            var mockPersonRepository = new Mock<IRepository<Person>>();
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockFlightRepository = new Mock<IRepository<Flight>>();

            mockBookingRepository.Setup(x => x.GetAll()).Returns(bookings);

            var bookingService = new BookingService(Mapper.Instance, mockPersonRepository.Object, mockBookingRepository.Object, mockFlightRepository.Object);

            // Act 
            var expectedBooking = bookingService.GetBookingByNumber("Not existing number");

            // Assert
            Assert.IsNull(expectedBooking);
        }

        [Test]
        public void Create_WhenCorrectData_ShouldReturnSuccessfulResponse()
        {
            // Arrange 
            var flight = PrepareFlightDetails();
            var bookingPassenger = new BookingPassenger { Name = "Test", DateBirth = DateTime.Now.Date, Gender = Gender.Male, Address = "TestAddress", Email = "email@email.com" };
            var createBookingRequest = new CreateBookingRequest
            {
                FlightId = 1,
                Passengers = new List<BookingPassenger> { bookingPassenger }
            };

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Booking, BookingDto>());

            var mockPersonRepository = new Mock<IRepository<Person>>();
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockFlightRepository = new Mock<IRepository<Flight>>();

            mockFlightRepository.Setup(x => x.Get(It.Is<int>(m => m == flight.Id))).Returns(flight);
            mockBookingRepository.Setup(x => x.GetAll()).Returns(new List<Booking>());
            mockPersonRepository.Setup(x => x.GetAll()).Returns(new List<Person>());

            var bookingService = new BookingService(Mapper.Instance, mockPersonRepository.Object, mockBookingRepository.Object, mockFlightRepository.Object);

            // Act 
            var expectedCreateBookingResponse = bookingService.CreateBooking(createBookingRequest);

            // Assert
            Assert.IsTrue(expectedCreateBookingResponse.IsSuccessful);
            Assert.IsNotNull(expectedCreateBookingResponse.BookingNumber);
            Assert.IsNull(expectedCreateBookingResponse.ErrorMessage);
        }

        [Test]
        public void Create_WhenFlightDoesNotExsit_ShouldReturnAppropriateUnsuccessfulResponse()
        {
            // Arrange 
            var expectedErrorMessage = "Flight with specified ID does not exist.";

            var flight = PrepareFlightDetails();
            var bookingPassenger = new BookingPassenger { Name = "Test", DateBirth = DateTime.Now.Date, Gender = Gender.Male, Address = "TestAddress", Email = "email@email.com" };
            var createBookingRequest = new CreateBookingRequest
            {
                FlightId = 1,
                Passengers = new List<BookingPassenger> { bookingPassenger }
            };

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Booking, BookingDto>());

            var mockPersonRepository = new Mock<IRepository<Person>>();
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockFlightRepository = new Mock<IRepository<Flight>>();

            mockFlightRepository.Setup(x => x.GetAll()).Returns(new List<Flight>());
            mockBookingRepository.Setup(x => x.GetAll()).Returns(new List<Booking>());
            mockPersonRepository.Setup(x => x.GetAll()).Returns(new List<Person>());

            var bookingService = new BookingService(Mapper.Instance, mockPersonRepository.Object, mockBookingRepository.Object, mockFlightRepository.Object);

            // Act 
            var expectedCreateBookingResponse = bookingService.CreateBooking(createBookingRequest);

            // Assert
            Assert.IsFalse(expectedCreateBookingResponse.IsSuccessful);
            Assert.IsNull(expectedCreateBookingResponse.BookingNumber);
            Assert.AreEqual(expectedCreateBookingResponse.ErrorMessage, expectedErrorMessage);
        }

        private Flight PrepareFlightDetails()
        {
            return new Flight
            {
                Id = 1,
                Number = "111",
                Carrier = new Airline { Code = "1", Name = "1", Address = "A1" },
                DepartureAirport = new Airport { Code = "A1", Id = 1, City = "C1", Country = "CC1" },
                DepartureDate = DateTime.Now,
                ArrivalAirport = new Airport { Code = "A2", Id = 2, City = "C2", Country = "CC2" },
                ArrivalDate = DateTime.Now,
                Price = 111
            };
        }
    }
}
