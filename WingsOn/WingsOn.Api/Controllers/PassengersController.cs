using Microsoft.AspNetCore.Mvc;
using WingsOn.Api.Infrastructure;
using System.Collections.Generic;
using WingsOn.Api.Models.Passenger;
using Microsoft.AspNetCore.JsonPatch;
using WingsOn.Infrastructure.Interfaces;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Api.Controllers
{
    [Route("wingsonapi/[controller]")]
    [ApiController]
    [WingsOnExceptionFilter]
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
            return Ok(_passengerService.GetPassengerById(id));
        }

        [HttpGet]
        public ActionResult<List<PassengerDto>> Get([FromQuery]GetPassengersRequest getPassengersRequest)
        {
            return Ok(_passengerService.GetPassengers(getPassengersRequest));
        }

        [HttpPatch("{id}")]
        public ActionResult Patch(int id, [FromBody]JsonPatchDocument<PassengerDto> passengerPatch)
        {
            _passengerService.UpdatePassenger(id, passengerPatch);
            return Ok();
        }
    }
}