using Application.Common;
using Application.DTO.Admin;
using Application.DTO.Event;
using Application.Interfaces;
using MediatR;


namespace Application.Commands.Admin.Event.GetAllEvents
{
    public class GetAllEventsCommandHandler : IRequestHandler<GetAllEventsCommand,Result<List<AdminEventDTO>>>
    {
        private readonly IAdminService _adminService;

        public GetAllEventsCommandHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<Result<List<AdminEventDTO>>> Handle(GetAllEventsCommand command,CancellationToken token)
        {
            var result = await _adminService.GetAllEventsAsync(command.Page,command.PageSize);
            if (!result.Success)
            {
                return Result<List<AdminEventDTO>>.IsFailure(result.Errors);
            }
            return result;
        }
    }
}
