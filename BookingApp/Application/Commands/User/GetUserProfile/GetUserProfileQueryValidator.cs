using FluentValidation;

namespace Application.Commands.User.GetUserProfile
{
    public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
    {
        public GetUserProfileQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Cannot be empty.")
                .NotNull().WithMessage("Cannot be null");
        }
    }
}