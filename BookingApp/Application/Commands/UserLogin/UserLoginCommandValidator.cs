using Application.Interfaces;
using FluentValidation;

namespace Application.Commands.UserLogin
{
    public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
    {
        private readonly IUserService _userService;

        public UserLoginCommandValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(x => x.Identifier)
                .NotEmpty()
                .WithMessage("There is no login or email.");

            When(x => x.Identifier.Contains('@'), () => {
                RuleFor(x => x.Identifier)
                .EmailAddress()
                .WithMessage("This is not a valid email address.")
                .MustAsync(async (email, token) => 
                { 
                    var isUserExist = await _userService.IsUserExistByEmailAsync(email);
                    return isUserExist.Value;
                })
                .WithMessage("User with this email does not exist.");
            });

            When(x => !x.Identifier.Contains('@'), () =>
            {
                RuleFor(x => x.Identifier)
                .MinimumLength(8)
                .WithMessage("Username is too small.")
                .MustAsync(async (login, token) =>
                {
                    var isUserExist = await _userService.IsUserExistByNameAsync(login);
                    return isUserExist.Value;
                })
                .WithMessage("User with this login does not exist.");
            });

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is empty")
                .MinimumLength(4)
                .WithMessage("Password is too small");
        }
    }
}
