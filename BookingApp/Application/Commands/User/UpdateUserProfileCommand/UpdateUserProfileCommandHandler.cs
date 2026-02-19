using Application.Common;
using Application.DTO.User;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.User.UpdateUserProfileCommand
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Result<UserProfileDTO>>
    {
        private readonly IUserService _userService;

        public UpdateUserProfileCommandHandler(IUserService usersrvice)
        {
            _userService = usersrvice;
        }

        public async Task<Result<UserProfileDTO>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            return await _userService.UpdateUserProfileAsync(request.UserId!, request.Username);
        }
    }
}
