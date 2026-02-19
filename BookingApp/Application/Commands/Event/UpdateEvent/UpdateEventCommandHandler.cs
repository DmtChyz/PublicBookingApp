using Application.Common;
using Application.DTO.Event;
using Application.Interfaces;
using Application.Parameters.Events;
using MediatR;

namespace Application.Commands.Event.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand,Result<EventDTO>>    
    {
        private readonly IEventService _eventService;

        public UpdateEventCommandHandler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Result<EventDTO>> Handle(UpdateEventCommand command,CancellationToken token)
        {
            var parameters = new UpdateEventParameters
            {
                Id = command.Id,
                RequestorID = command.RequestorID,
                Title = command.Title,
                Description = command.Description,
                EventDate = command.EventDate,
                MaxAttendees = command.MaxAttendees,
                ImageUrl = command.ImageUrl,
                Address = command.Address
            };

            var isUpdated = await _eventService.UpdateEventAsync(parameters);

            if (!isUpdated.Success)
            {
                return Result<EventDTO>.IsFailure(isUpdated.Errors);
            }

            return isUpdated;
        }
    }
}
