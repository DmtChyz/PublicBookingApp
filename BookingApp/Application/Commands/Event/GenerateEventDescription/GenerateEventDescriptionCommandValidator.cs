using Application.Commands.Event.GenerateEventDescription;
using FluentValidation;

namespace Application.Validators.Event
{
    public class GenerateEventDescriptionCommandValidator : AbstractValidator<GenerateEventDescriptionCommand>
    {
        public GenerateEventDescriptionCommandValidator()
        {
            RuleFor(x => x.UserPrompt)
                .NotEmpty().WithMessage("A prompt for the AI is required.");

            RuleFor(x => x.Title)
                .NotNull().WithMessage("Title cannot be null.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.City)
                .NotNull().WithMessage("City cannot be null.")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.Country)
                .NotNull().WithMessage("Country cannot be null.")
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.")
                .When(x => x.Price.HasValue);
        }
    }
}