using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WingsOn.Api.Models.Passenger;
using Microsoft.AspNetCore.JsonPatch;
using WingsOn.Infrastructure.Interfaces;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Api.Controllers
{
    [Route("wingsonapi/[controller]")]
    [ApiController]
    public class PassengersController : Controller
    {
        private readonly IPassengerService _passengerService;

        public PassengersController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        [HttpGet("{id}")]
        public ActionResult<PassengerDto> Get(int id)
        {
            var passenger = _passengerService.GetPassengerById(id);

            if (passenger == null)
            {
                return NotFound("Passenger with speicified ID is not found.");
            }

            return Ok(passenger);
        }

        [HttpGet]
        public ActionResult<List<PassengerDto>> Get([FromQuery]GetPassengersRequest getPassengersRequest)
        {
            try
            {
                var passengers = _passengerService.GetPassengers(getPassengersRequest);

                if (passengers == null)
                {
                    return NotFound("Passengers are not found.");
                }

                return Ok(passengers);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch("{id}")]
        public ActionResult<UpdatePassengerResponse> Patch(int id, [FromBody]JsonPatchDocument<PassengerDto> passengerPatch)
        {
            var passenger = _passengerService.GetPassengerById(id);
            if (passenger == null)
            {
                return NotFound("Passenger with speicifed ID is not found.");
            }

            passengerPatch.ApplyTo(passenger);

            var updatePassengerRequest = new UpdatePassengerRequest
            {
                PassengerId = id,
                Passenger = passenger
            };

            var updatePassengerResponse = _passengerService.UpdatePassenger(updatePassengerRequest);
            if (!updatePassengerResponse.IsSuccessful)
            {
                return BadRequest(updatePassengerResponse.ErrorMessage);
            }

            return Ok(updatePassengerResponse);
        }
    }
}