using Microsoft.AspNetCore.Mvc;
using WingsOn.Api.Models.Booking;
using WingsOn.Infrastructure.Interfaces;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Api.Controllers
{
    [Route("wingsonapi/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{bookingNumber}")]
        public ActionResult<BookingDto> Get(string bookingNumber)
        {
            var booking = _bookingService.GetBookingByNumber(bookingNumber);

            if (booking == null)
            {
                return NotFound("Booking with specified number is not found.");
            }

            return booking;
        }

        [HttpPut]
        public ActionResult<CreateBookingResponse> Put([FromBody] CreateBookingRequest createBookingRequest)
        {
            var createBookingResponse = _bookingService.CreateBooking(createBookingRequest);

            if (!createBookingResponse.IsSuccessful)
            {
                return BadRequest($"Booking creation failed. Error: {createBookingResponse.ErrorMessage}");
            }

            return Ok(createBookingResponse);
        }
    }
}