using Application.Common;
using Application.DTO.Admin;
using Application.DTO.Event;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Event.GetEventById
{
    public class GetEventByIdCommandHandler : IRequestHandler<GetEventByIdCommand,Result<AdminEventDTO>>
    {
        private readonly IAdminService _adminService;

        public GetEventByIdCommandHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<Result<AdminEventDTO>> Handle(GetEventByIdCommand command,CancellationToken token)
        {
            var result = await _adminService.GetEventByIdAsync(command.EventId);
            if (!result.Success)
            {
                return Result<AdminEventDTO>.IsFailure(result.Errors);
            }
            return result;
        }
    }
}
