using Application.Commands.Booking.CreateBooking;
using Application.Commands.Booking.DeleteBooking;
using Application.Commands.Booking.GetBookingById;
using Application.Commands.Booking.GetProfileBookings;
using Application.Commands.Booking.UpdateBooking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/booking")]
    public class BookingController : Controller
    {
        private readonly ISender _sender;

        public BookingController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            command.ClientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _sender.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return Ok(result.Value);
        }

        [HttpPatch("{bookingId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateBooking([FromBody] UpdateBookingCommand command, [FromRoute] int bookingId)
        {
            command.BookingId = bookingId;
            command.ClientId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _sender.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return Ok(result.Value);
        }

        [HttpGet("{bookingID}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetBookingById([FromRoute] int bookingID)
        {
            var command = new GetBookingByIdCommand()
            {
                BookingId = bookingID,
                RequestorId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            var result = await _sender.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return Ok(result.Value);
        }

        [HttpDelete("{bookingId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteBookingById([FromRoute] int bookingId)
        {
            var command = new DeleteBookingCommand()
            {
                BookingId = bookingId,
                RequestorId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            var result = await _sender.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return Ok(result.Value);
        }

        [HttpGet("mybookings")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GetProfileBookings()
        {
            var query = new GetProfileBookingsQuery()
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            var result = await _sender.Send(query);

            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return Ok(result.Value);
        }
    }
}