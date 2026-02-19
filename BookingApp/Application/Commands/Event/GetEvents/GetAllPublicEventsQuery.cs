using Application.Common;
using Application.DTO.Event;
using MediatR;
using System.Collections.Generic;

namespace Application.Queries.Event.GetAllPublicEvents
{
    public class GetAllPublicEventsQuery : IRequest<Result<List<PublicEventSummaryDTO>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; } // "date", "price"
        public string? SortOrder { get; set; } // "asc", "desc"
    }
}