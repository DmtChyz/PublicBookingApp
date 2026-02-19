using Application.Commands.Event.CreateEvent;
using Application.Commands.Event.UpdateEvent;
using Application.Commands.Event.DeleteEvent;
using Application.DTO.Event;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Commands.Event.GetEventById;
using Application.Queries.Event.GetAllPublicEvents;
using Application.Queries.Event.GetMyEvents;
using Application.Commands.Event.GenerateEventDescription;

namespace API.Controllers
{
    [ApiController]
    [Route("api/event")]
    public class EventController : Controller
    {
        private readonly ISender _sender;

        public EventController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPublicEvents([FromQuery] GetAllPublicEventsQuery querry)
        {
            var result = await _sender.Send(querry);
            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }
            return Ok(result.Value);
        }
        [HttpGet("my-events")]
        [Authorize]
        public async Task<IActionResult> GetMyEvents()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetMyEventsQuery { OwnerId = userId };

            var result = await _sender.Send(query);

            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return Ok(result.Value);
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventCommand command)
        {
            command.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (command.OwnerId == null) return BadRequest(new { errors = new { Auth = new[] { "Invalid authentication token." } } });

            var result = await _sender.Send(command);
            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }

            var eventDto = result.Value;
            return CreatedAtAction(nameof(GetEventById), new { id = eventDto.Id }, eventDto);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateEvent([FromRoute] int id, [FromBody] UpdateEventDTO eventDataToUpdate)
        {
            var command = new UpdateEventCommand
            {
                Id = id,
                RequestorID = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Title = eventDataToUpdate.Title,
                Description = eventDataToUpdate.Description,
                EventDate = eventDataToUpdate.EventDate,
                MaxAttendees = eventDataToUpdate.MaxAttendees,
                ImageUrl = eventDataToUpdate.ImageUrl,
                Address = eventDataToUpdate.Address
            };

            var result = await _sender.Send(command);
            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }
            return Ok(result.Value);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEventById([FromRoute] int id)
        {
            var command = new GetEventByIdCommand() { EventId = id };

            var result = await _sender.Send(command);
            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }
            return Ok(result.Value);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var command = new DeleteEventCommand
            {
                EventId = id,
                RequestorId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            var result = await _sender.Send(command);
            if (!result.Success)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return NoContent();
        }

        [HttpPost("generateDescription")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> GenerateEventDescription([FromBody] GenerateEventDescriptionCommand command)
        {
            var result = await _sender.Send(command);
            if (!result.Success)
            {
                return BadRequest(new {errors =  result.Errors});
            }
            return Ok(result.Value);
        }
    }
}