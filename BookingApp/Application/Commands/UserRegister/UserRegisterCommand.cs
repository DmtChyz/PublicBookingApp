using Application.Common;
using Application.DTO;
using MediatR;

namespace Application.Commands.RegisterUser
{
    public class UserRegisterCommand : IRequest<Result<AuthentificationResultDTO>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}