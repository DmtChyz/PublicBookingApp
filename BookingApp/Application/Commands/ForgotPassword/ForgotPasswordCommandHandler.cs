using Application.Common;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand,Result<bool>>
    {
        private readonly IUserService _userService;

        public ForgotPasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Result<bool>> Handle(ForgotPasswordCommand command,CancellationToken token)
        {
            var result = await _userService.ForgotPasswordAsync(command.Email);
            if (!result.Success)
            {
                return Result<bool>.IsFailure(result.Errors);
            }
            return Result<bool>.IsSuccess(true);
        }
    }
}
