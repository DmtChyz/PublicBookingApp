using Application.Common;
using Application.DTO.Admin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.User.GetAllUsers
{
    public class GetAllUsersCommand : IRequest<Result<List<UserDTO>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
