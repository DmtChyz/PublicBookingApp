using Application.Common;
using Application.DTO.Event;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Event.GetAllPublicEvents
{
    public class GetAllPublicEventsQueryHandler : IRequestHandler<GetAllPublicEventsQuery, Result<List<PublicEventSummaryDTO>>>
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public GetAllPublicEventsQueryHandler(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        public async Task<Result<List<PublicEventSummaryDTO>>> Handle(GetAllPublicEventsQuery request, CancellationToken cancellationToken)
        {
            return await _eventService.GetAllPublicEventsAsync(request.Page, request.PageSize,request.SortBy,request.SortOrder);
        }
    }
}