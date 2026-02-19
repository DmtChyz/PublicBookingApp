using Application.Common;
using Application.DTO.Event;
using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.Queries.Event.GetMyEvents
{
    public class GetMyEventsQuery : IRequest<Result<IEnumerable<PublicEventSummaryDTO>>>
    {
        [JsonIgnore]
        public string OwnerId { get; set; }
    }
}