using Moq;
using System;
using AutoMapper;
using WingsOn.Dal;
using System.Linq;
using WingsOn.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using WingsOn.Api.Models.Passenger;
using WingsOn.Api.Models.Exceptions;
using WingsOn.Infrastructure.Services;
using WingsOn.Api.Models.Common.Entities;

namespace Tests
{
    public class PassengerServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Get_WhenCorrectData_ShouldGetProperly()
        {
            // Arrange 
            var person = new Person { Id = 1, Name = "Test", DateBirth = DateTime.Now.Date, Gender = GenderType.Male, Address = "TestAddress", Email = "email@email.com" };

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Person, PassengerDto>());

            var passenger = Mapper.Map<Person, PassengerDto>(person);

            var mockPersonRepository = new Mock<IRepository<Person>>();
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockFlightRepository = new Mock<IRepository<Flight>>();

            mockPersonRepository.Setup(x => x.Get(It.Is<int>(m => m == person.Id))).Returns(person);
            
            var passengerService = new PassengerService(Mapper.Instance, mockPersonRepository.Object, mockBookingRepository.Object, mockFlightRepository.Object);

            // Act 
            var expectedPassenger = passengerService.GetPassengerById(person.Id);

            // Assert
            Assert.AreEqual(expectedPassenger.Id, passenger.Id);
            Assert.AreEqual(expectedPassenger.Name, passenger.Name);
            Assert.AreEqual(expectedPassenger.Address, passenger.Address);
            Assert.AreEqual(expectedPassenger.DateBirth, passenger.DateBirth);
            Assert.AreEqual(expectedPassenger.Email, passenger.Email);
            Assert.AreEqual(expectedPassenger.Gender, passenger.Gender);
        }

        [Test]
        public void Get_WhenIncorrectData_ShouldThrowAppropriateException()
        {
            // Arrange 
            var person = new Person { Id = 1, Name = "Test", DateBirth = DateTime.Now.Date, Gender = GenderType.Male, Address = "TestAddress", Email = "email@email.com" };

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Person, PassengerDto>());

            var mockPersonRepository = new Mock<IRepository<Person>>();
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockFlightRepository = new Mock<IRepository<Flight>>();

            mockPersonRepository.Setup(x => x.Get(It.Is<int>(m => m == person.Id))).Returns((Person)null);

            var passengerService = new PassengerService(Mapper.Instance, mockPersonRepository.Object, mockBookingRepository.Object, mockFlightRepository.Object);

            // Act & Assert
            var exception = Assert.Throws<WingsOnNotFoundException>(() => passengerService.GetPassengerById(person.Id));
            Assert.AreEqual(exception.Message, "Passenger with specified ID is not found.");
        }

        [Test]
        public void GetAll_WhenNoPassengers_ShouldThrowAppropriateException()
        {
            // Arrange
            var getPassengersRequest = new GetPassengersRequest();

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Person, PassengerDto>());

            var mockPersonRepository = new Mock<IRepository<Person>>();
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockFlightRepository = new Mock<IRepository<Flight>>();

            mockPersonRepository.Setup(x => x.GetAll()).Returns((IEnumerable<Person>)null);

            var passengerService = new PassengerService(Mapper.Instance, mockPersonRepository.Object, mockBookingRepository.Object, mockFlightRepository.Object);

            // Act & Assert
            var exception = Assert.Throws<WingsOnNotFoundException>(() => passengerService.GetPassengers(getPassengersRequest));
            Assert.AreEqual(exception.Message, "Passengers are not found.");
        }

        [Test]
        public void GetAll_WhenGenderIsSet_ShouldReturnPassengersByGender()
        {
            // Arrange
            var getPassengersRequest = new GetPassengersRequest
            {
                Gender = Gender.Male
            };

            var passengersByGender = new List<Person>
            { 
                new Person { Id = 1, Name = "Test1", DateBirth = DateTime.Now.Date, Gender = GenderType.Male, Address = "TestAddress", Email = "email@email.com" },
                new Person { Id = 2, Name = "Test2", DateBirth = DateTime.Now.Date, Gender = GenderType.Female, Address = "TestAddress", Email = "email@email.com" },
                new Person { Id = 3, Name = "Test3", DateBirth = DateTime.Now.Date, Gender = GenderType.Male, Address = "TestAddress", Email = "email@email.com" }
            };

            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Person, PassengerDto>());

            var malePassengers = Mapper.Map<List<Person>, List<PassengerDto>>(passengersByGender.Where(pas => pas.Gender == GenderType.Male).ToList());

            var mockPersonRepository = new Mock<IRepository<Person>>();
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockFlightRepository = new Mock<IRepository<Flight>>();

            mockPersonRepository.Setup(x => x.GetAll()).Returns(passengersByGender);

            var passengerService = new PassengerService(Mapper.Instance, mockPersonRepository.Object, mockBookingRepository.Object, mockFlightRepository.Object);

            // Act 
            var passengers = passengerService.GetPassengers(getPassengersRequest);

            // Assert
            Assert.AreEqual(malePassengers.Count, passengers.Count);
            Assert.AreEqual(malePassengers.Select(pas => pas.Id).ToList(), passengers.Select(pas => pas.Id).ToList());
        }
    }
}