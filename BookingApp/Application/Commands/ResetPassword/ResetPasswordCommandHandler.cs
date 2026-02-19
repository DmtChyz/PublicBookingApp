using Application.Common;
using Application.DTO;
using Application.Interfaces;
using MediatR;

namespace Application.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<bool>>
    {
        private readonly IUserService _userService;

        public ResetPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var isPasswordSuccesfullyReset = await _userService.ResetPasswordAsync(request.Email,request.Token,request.NewPassword);
            if (!isPasswordSuccesfullyReset.Success)
            {   // only with internal errors which is impossible
                return Result<bool>.IsFailure("Something went wrong. Pleaste try again later.");
            }
            return Result<bool>.IsSuccess(true);
        }
    }
}
