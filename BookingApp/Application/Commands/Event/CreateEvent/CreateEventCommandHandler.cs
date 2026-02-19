using Application.Common;
using Application.DTO.Event;
using Application.Interfaces;
using Application.Parameters;
using Application.Parameters.Events;
using MediatR;

namespace Application.Commands.Event.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand,Result<EventDTO>>
    {
        private readonly IEventService _eventService;

        public CreateEventCommandHandler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Result<EventDTO>> Handle(CreateEventCommand request,CancellationToken token)
        {
            var parameters = new CreateEventParameters
            {
                Title = request.Title,
                Description = request.Description,
                EventDate = request.EventDate,
                MaxAttendees = request.MaxAttendees,
                ImageUrl = request.ImageUrl,
                OwnerId = request.OwnerId,
                Address = request.Address,
                Price = request.Price
            };
            var result = await _eventService.CreateEventAsync(parameters);
            if (!result.Success)
            {
                return Result<EventDTO>.IsFailure(result.Errors);
            }

            return result;
        }
    }
}
