using Application.Common;
using Application.DTO.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commands.User.UpdateUserProfileCommand
{
    public class UpdateUserProfileCommand : IRequest<Result<UserProfileDTO>>
    {
        [JsonIgnore]
        public string? UserId { get; set; }

        public string? Username { get; set; }
    }
}
