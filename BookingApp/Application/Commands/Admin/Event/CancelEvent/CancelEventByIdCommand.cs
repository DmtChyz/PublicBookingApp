using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Event.CancelEvent
{
    public class CancelEventByIdCommand : IRequest<Result<bool>>
    {
        public int EventId { get; set; }
    }
}
