using Application.Common;
using Application.DTO.Admin;
using MediatR;


namespace Application.Commands.Admin.Event.GetAllEvents
{
    public class GetAllEventsCommand : IRequest<Result<List<AdminEventDTO>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
