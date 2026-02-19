using Application.Common;
using Application.DTO;
using Application.Interfaces;
using Application.Commands;
using MediatR;

namespace Application.Commands.RegisterUser
{
    public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand, Result<AuthentificationResultDTO>>
    {
        private readonly IUserService _userService;

        public UserRegisterCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<AuthentificationResultDTO>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var userToUserRegisterDTO = new UserRegisterDTO
            {
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
            };

            var userToken = await _userService.RegisterUser(userToUserRegisterDTO);
            if (!userToken.Success) return Result<AuthentificationResultDTO>.IsFailure(userToken.Errors);

            return userToken;
        }
    }
}