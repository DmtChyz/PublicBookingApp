using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commands.Event.DeleteEvent
{
    public class DeleteEventCommand : IRequest<Result<bool>>
    {
        public int EventId { get; set; }
        [JsonIgnore]
        public string RequestorId { get; set; }
    }
}
