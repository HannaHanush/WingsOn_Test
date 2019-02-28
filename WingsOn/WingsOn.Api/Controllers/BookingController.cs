using Microsoft.AspNetCore.Mvc;
using WingsOn.Api.Models.Booking;
using WingsOn.Api.Infrastructure;
using WingsOn.Infrastructure.Interfaces;
using WingsOn.Api.Models.Common.Entities;

namespace WingsOn.Api.Controllers
{
    [Route("wingsonapi/[controller]")]
    [ApiController]
    [WingsOnExceptionFilter]
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
            return Ok(_bookingService.GetBookingByNumber(bookingNumber));
        }

        [HttpPut]
        public ActionResult<CreateBookingResponse> Put([FromBody] CreateBookingRequest createBookingRequest)
        {
            return Ok(_bookingService.CreateBooking(createBookingRequest));
        }
    }
}