using Application.Common;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Event.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand,Result<bool>>
    {
        private readonly IEventService _eventService;

        public DeleteEventCommandHandler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Result<bool>> Handle(DeleteEventCommand command, CancellationToken token)
        {
            var isDeletedSuccessfully = await _eventService.DeleteEventAsync(command.EventId,command.RequestorId);
            if (!isDeletedSuccessfully.Success) return Result<bool>.IsFailure(isDeletedSuccessfully.Errors);
            return Result<bool>.IsSuccess(true);
        }
    }
}
