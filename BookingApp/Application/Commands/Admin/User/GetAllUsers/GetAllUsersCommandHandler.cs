using Application.Common;
using Application.DTO.Admin;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.User.GetAllUsers
{
    public class GetAllUsersCommandHandler : IRequestHandler<GetAllUsersCommand,Result<List<UserDTO>>>
    {
        private readonly IAdminService _adminService;

        public GetAllUsersCommandHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<Result<List<UserDTO>>> Handle(GetAllUsersCommand command,CancellationToken token)
        {
            var result = await _adminService.GetAllUsersAsync(command.Page,command.PageSize);
            if (!result.Success)
            {
                return Result<List<UserDTO>>.IsFailure(result.Errors);
            }
            return result;
        }
    }
}
