using Application.Common;
using Application.DTO.Admin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.User.GetUserById
{
    public class GetUserByIdCommand : IRequest<Result<UserDTO>>
    {
        public string UserId { get; set; }
    }
}
