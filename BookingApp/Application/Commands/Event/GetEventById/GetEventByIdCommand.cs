using Application.Common;
using Application.DTO.Event;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Event.GetEventById
{
    public class GetEventByIdCommand : IRequest<Result<EventDTO>>
    {
        public int EventId { get; set; }
    }
}
