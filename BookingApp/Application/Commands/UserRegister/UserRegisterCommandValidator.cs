using Application.Commands.RegisterUser;
using Application.Interfaces;
using FluentValidation;

namespace Application.Commands.UserRegister
{
    public class UserRegisterCommandValidator : AbstractValidator<UserRegisterCommand>
    {
        private readonly IUserService _userService;

        public UserRegisterCommandValidator(IUserService userService)
        {
            _userService = userService;

            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(8).WithMessage("Username must be at least 8 characters long.")
                .MaximumLength(20).WithMessage("Username must not exceed 20 characters.")
                .Matches(@"^[\p{L}\p{N}_-]+$").WithMessage("Username can only contain letters, numbers, underscore, and hyphen.")
                .MustAsync(async (username, cancellationToken) => {
                    var result = await _userService.IsUserExistByNameAsync(username);
                    return !result.Value;
                })
                .WithMessage("This username is already taken.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(32).WithMessage("Password must not exceed 32 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[#?!@$%^&*-+]").WithMessage("Password must contain at least one special character. \" #?!@$%^&*-+ \" ");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email cannot be empty.")
                .MaximumLength(254)
                .EmailAddress()
                .WithMessage("This email is not an address.")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")
                .WithMessage("Incorrect email format.")
                .MustAsync(async (email, cancellationToken) => {
                    var result = await _userService.IsUserExistByEmailAsync(email);
                    return !result.Value;
                })
                .WithMessage("This email address is already registered.");
        }
    }
}