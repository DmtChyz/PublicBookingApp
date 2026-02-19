using Application.Common;
using Application.DTO.Event;
using Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries.Event.GetMyEvents
{
    public class GetMyEventsQueryHandler : IRequestHandler<GetMyEventsQuery, Result<IEnumerable<PublicEventSummaryDTO>>>
    {
        private readonly IEventService _eventService;

        public GetMyEventsQueryHandler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Result<IEnumerable<PublicEventSummaryDTO>>> Handle(GetMyEventsQuery request, CancellationToken cancellationToken)
        {
            var result = await _eventService.GetMyEventsAsync(request.OwnerId);

            if (!result.Success)
            {
                return Result<IEnumerable<PublicEventSummaryDTO>>.IsFailure(result.Errors);
            }

            return Result<IEnumerable<PublicEventSummaryDTO>>.IsSuccess(result.Value);
        }
    }
}