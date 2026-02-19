using Application.Common;
using Application.DTO.User;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.User.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<Result<UserProfileDTO>>
    {
        [JsonIgnore]
        public string UserId { get; set; }
    }
}