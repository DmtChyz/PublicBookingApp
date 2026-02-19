using Application.Commands.Admin.Booking.AllBookings;
using Application.Commands.Admin.Booking.CancelBookingById;
using Application.Commands.Admin.Booking.GetBookingById;
using Application.Commands.Admin.Event.CancelEvent;
using Application.Commands.Admin.Event.GetAllEvents;
using Application.Commands.Admin.Event.GetEventById;
using Application.Commands.Admin.User.GetAllUsers;
using Application.Commands.Admin.User.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly ISender _sender;

        public AdminController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetAllEvents([FromBody] GetAllEventsCommand command)
        {
            var result = await _sender.Send(command);
            return result.Success ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet("events/{id:int}")]
        public async Task<IActionResult> GetEventById([FromRoute] int id)
        {
            var command = new GetEventByIdCommand { EventId = id };
            var result = await _sender.Send(command);
            return result.Success ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPatch("events/{id:int}/cancel")]
        public async Task<IActionResult> CancelEvent([FromRoute] int id)
        {
            var command = new CancelEventByIdCommand { EventId = id };
            var result = await _sender.Send(command);
            return result.Success ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings([FromBody] GetAllBookingsCommand command)
        {
            var result = await _sender.Send(command);
            return result.Success ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet("bookings/{id:int}")]
        public async Task<IActionResult> GetBookingById([FromRoute] int id)
        {
            var command = new GetBookingByIdCommand { BookingId = id };
            var result = await _sender.Send(command);
            return result.Success ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPatch("bookings/{id:int}/cancel")]
        public async Task<IActionResult> CancelBooking([FromRoute] int id)
        {
            var command = new CancelBookingByIdCommand { BookingId = id };
            var result = await _sender.Send(command);
            return result.Success ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers([FromBody] GetAllUsersCommand command)
        {
            var result = await _sender.Send(command);
            return result.Success ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var command = new GetUserByIdCommand { UserId = id };
            var result = await _sender.Send(command);
            return result.Success ? Ok(result.Value) : BadRequest(result.Errors);
        }
    }
}