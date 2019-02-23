using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WingsOn.Infrastructure.Interfaces;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Api.Controllers
{
    [Route("wingsonapi/[controller]")]
    [ApiController]
    public class FlightsController : Controller
    {
        private readonly IPassengerService _passengerService;

        public FlightsController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpGet]
        [Route("GetFlightPassengers/{flightNumber}")]
        public ActionResult<List<PassengerDto>> GetFlightPassengers(string flightNumber)
        {
            try
            {
                var passengers = _passengerService.GetFlightPassengers(flightNumber);

                if (passengers == null)
                {
                    return NotFound($"Passengers of {flightNumber} flight are not found.");
                }

                return passengers;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}