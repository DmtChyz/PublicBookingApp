using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.User.UpdateUserProfileCommand
{
    public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator()
        {

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(8).WithMessage("Username must be at least 8 characters long.")
                .MaximumLength(20).WithMessage("Username must not exceed 20 characters.")
                .Matches(@"^[\p{L}\p{N}_-]+$").WithMessage("Username can only contain letters, numbers, underscore, and hyphen.");
        }
    }
}
