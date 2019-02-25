using Moq;
using System;
using AutoMapper;
using WingsOn.Dal;
using WingsOn.Domain;
using NUnit.Framework;
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
            var passenger = new PassengerDto { Name = "Test", DateBirth = DateTime.Now.Date, Gender = Gender.Male, Address = "TestAddress", Email = "email@email.com" };

            Mapper.Initialize(cfg => cfg.CreateMap<Person, PassengerDto>());

            var mockPersonRepository = new Mock<IRepository<Person>>();
            var mockBookingRepository = new Mock<IRepository<Booking>>();
            var mockFlightRepository = new Mock<IRepository<Flight>>();

            mockPersonRepository.Setup(x => x.Get(It.Is<int>(m => m == person.Id))).Returns(person);
            
            var passengerService = new PassengerService(Mapper.Instance, mockPersonRepository.Object, mockBookingRepository.Object, mockFlightRepository.Object);

            // Act 
            var expectedPassenger = passengerService.GetPassengerById(person.Id);

            // Assert
            Assert.AreEqual(expectedPassenger.Name, passenger.Name);
            Assert.AreEqual(expectedPassenger.Address, passenger.Address);
            Assert.AreEqual(expectedPassenger.DateBirth, passenger.DateBirth);
            Assert.AreEqual(expectedPassenger.Email, passenger.Email);
            Assert.AreEqual(expectedPassenger.Gender, passenger.Gender);
        }
    }
}