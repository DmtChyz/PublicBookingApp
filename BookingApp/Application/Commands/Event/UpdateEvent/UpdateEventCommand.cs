using Application.Common;
using Application.DTO.Event;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Event.UpdateEvent
{
    public class UpdateEventCommand : IRequest<Result<EventDTO>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string RequestorID { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDate { get; set; }
        public int? MaxAttendees { get; set; }
        public string? ImageUrl { get; set; }

        public UpdateAddressDTO? Address { get; set; }
    }
}
 