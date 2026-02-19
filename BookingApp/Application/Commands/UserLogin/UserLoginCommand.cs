using Application.Common;
using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserLogin
{
    public class UserLoginCommand : IRequest<Result<AuthentificationResultDTO>>
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
    }
}
