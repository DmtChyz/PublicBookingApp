using Application.Common;
using Application.DTO;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserLogin
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand,Result<AuthentificationResultDTO>>
    {
        private readonly IUserService _userService;

        public UserLoginCommandHandler(IUserService userService)
        {
            _userService =  userService;
        }

        public async Task<Result<AuthentificationResultDTO>> Handle(UserLoginCommand request,CancellationToken token)
        {
            UserLoginDTO userToLogin = new UserLoginDTO();

            if (request.Identifier.Contains('@'))
            {
                userToLogin.Email = request.Identifier;
                userToLogin.Password = request.Password;
            }
            else
            {
                userToLogin.Name = request.Identifier;
                userToLogin.Password = request.Password;
            }

            var isPasswordCorrect = await _userService.LoginAsync(userToLogin);

            if(!isPasswordCorrect.Success) return Result<AuthentificationResultDTO>.IsFailure(isPasswordCorrect.Errors);

            return Result<AuthentificationResultDTO>.IsSuccess(isPasswordCorrect.Value);
        }

    }
}
