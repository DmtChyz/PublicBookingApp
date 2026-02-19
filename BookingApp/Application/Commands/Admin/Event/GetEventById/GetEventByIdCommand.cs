using Application.Common;
using Application.DTO.Admin;
using Application.DTO.Event;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Event.GetEventById
{
    public class GetEventByIdCommand : IRequest<Result<AdminEventDTO>>
    {
        public int EventId { get; set; }
    }
}
