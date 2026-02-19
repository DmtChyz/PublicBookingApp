using Application.Common;
using Application.DTO.User;
using Application.Interfaces;
using MediatR;

namespace Application.Commands.User.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<UserProfileDTO>>
    {
        private readonly IUserService _userService;

        public GetUserProfileQueryHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<UserProfileDTO>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            return await _userService.GetUserProfileData(request.UserId);
        }
    }
}