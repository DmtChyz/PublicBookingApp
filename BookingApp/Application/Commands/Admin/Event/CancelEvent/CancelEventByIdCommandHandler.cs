using Application.Common;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Event.CancelEvent
{
    public class CancelEventByIdCommandHandler : IRequestHandler<CancelEventByIdCommand,Result<bool>>
    {
        private readonly IAdminService _adminService;

        public CancelEventByIdCommandHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<Result<bool>> Handle(CancelEventByIdCommand command,CancellationToken token)
        {
            var result = await _adminService.CancelEventAsync(command.EventId);
            if (!result.Success)
            {
                return Result<bool>.IsFailure(result.Errors);
            }
            return result;
        }
    }
}
