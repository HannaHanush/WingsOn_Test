using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WingsOn.Api.Infrastructure;
using WingsOn.Api.Models.Passenger;
using WingsOn.Infrastructure.Interfaces;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Api.Controllers
{
    [Route("wingsonapi/[controller]")]
    [ApiController]
    [WingsOnExceptionFilter]
    public class FlightsController : Controller
    {
        private readonly IPassengerService _passengerService;

        public FlightsController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpGet]
        [Route("{flightNumber}/Passengers")]
        public ActionResult<List<PassengerDto>> GetFlightPassengers(string flightNumber)
        {
            var getPassengersRequest = new GetPassengersRequest { FlightNumber = flightNumber };
            var passengers = _passengerService.GetPassengers(getPassengersRequest);

            if (passengers == null || !passengers.Any())
            {
                return NotFound($"Passengers of {flightNumber} flight are not found.");
            }

            return Ok(passengers);
        }
    }
}