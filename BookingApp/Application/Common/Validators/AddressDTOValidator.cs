using FluentValidation;
using Application.DTO.Event;

namespace Application.Common.Validators
{
    public class AddressDTOValidator : AbstractValidator<AddressDTO>
    {
        public AddressDTOValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(60).WithMessage("Country name cannot exceed 60 characters.");

            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage("Country code is required.")
                .Length(2).WithMessage("Country code must be exactly 2 characters long (ISO 3166-1 alpha-2).");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(85).WithMessage("City name cannot exceed 85 characters.");

            RuleFor(x => x.Venue)
                .NotEmpty().WithMessage("Venue is required.")
                .MaximumLength(100).WithMessage("Venue name cannot exceed 100 characters.");
        }
    }
}
