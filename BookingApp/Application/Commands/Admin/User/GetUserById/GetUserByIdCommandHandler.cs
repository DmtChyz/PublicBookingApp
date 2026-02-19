using Application.Common;
using Application.DTO.Admin;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.User.GetUserById
{
    public class GetUserByIdCommandHandler : IRequestHandler<GetUserByIdCommand,Result<UserDTO>>
    {
        private readonly IAdminService _adminService;

        public GetUserByIdCommandHandler(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<Result<UserDTO>> Handle(GetUserByIdCommand command,CancellationToken token)
        {
            var result = await _adminService.GetUserByIdAsync(command.UserId);
            if (!result.Success)
            {
                return Result<UserDTO>.IsFailure(result.Errors);
            }
            return result;
        }
    }
}
