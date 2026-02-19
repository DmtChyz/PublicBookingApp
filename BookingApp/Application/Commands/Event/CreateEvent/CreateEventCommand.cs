using Application.Common;
using MediatR;
using System.Text.Json.Serialization;
using Application.DTO.Event;

namespace Application.Commands.Event.CreateEvent
{
    public class CreateEventCommand : IRequest<Result<EventDTO>>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int MaxAttendees { get; set; }
        public string ImageUrl {  get; set; }
        public decimal Price { get; set; }

        [JsonIgnore]
        public string? OwnerId { get; set; }

        public AddressDTO Address { get; set;}
    }
}
