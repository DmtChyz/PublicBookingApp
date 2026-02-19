using Application.Commands.Event.GetEventById;
using Application.Common;
using Application.DTO.Event;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Event.GetEventById
{
    public class GetEventByIdCommandHandler : IRequestHandler<GetEventByIdCommand, Result<EventDTO>>
    {
        private readonly IEventService _eventService;

        public GetEventByIdCommandHandler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Result<EventDTO>> Handle(GetEventByIdCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventService.GetEventByIdAsync(request.EventId);

            if (!eventEntity.Success)
            {
                return Result<EventDTO>.IsFailure(eventEntity.Errors);
            }
            return eventEntity;
        }
    }
}