using Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Validators;

namespace Application.Commands.Event.CreateEvent
{
    public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
    {
        private readonly IEventService _eventService;

        public CreateEventCommandValidator(IEventService eventService)
        {
            _eventService = eventService;

            RuleFor(x => x.Title)
                .NotEmpty() // note: anonymous method takes argument that refers to RuleFor parameter ( in this case it'll be Title )
                .MinimumLength(8).WithMessage("Event title must be at least 8 characters long.")
                .MaximumLength(80).WithMessage("Event title must not exceed 80 characters.")
                .Matches(@"^[\p{L}\p{N}_ '.-]+$").WithMessage("Event title can only contain letters, numbers, underscore, and hyphen.")
                .MustAsync(async (title, cancellationToken) => {
                    var result = await _eventService.IsTitleUniqueAsync(title);
                    return result;
                })
                .WithMessage("Event title is already taken.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MinimumLength(24).WithMessage("Description must be at least 24 characters long.")
                .MaximumLength(768).WithMessage("Description must not exceed 768 characters.");

            RuleFor(x => x.EventDate)
                .NotEmpty().WithMessage("Event date cannot be empty.")
                .Must(date => date > DateTime.UtcNow.AddDays(1)).WithMessage("Event date must be in the future (at least tomorrow).");

            RuleFor(x => x.MaxAttendees)
                .NotEmpty().WithMessage("Max attendees cannot be empty.")
                .GreaterThan(0).WithMessage("Max attendees cannot be less than 1");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price cannot be less than 0.")
                .NotEmpty().WithMessage("Cannot be empty!")
                .NotNull().WithMessage("Cannot be null!");

            When(           /* condition to start */ 
                command => !string.IsNullOrWhiteSpace(command.ImageUrl),
                () => {

                    RuleFor(command => command.ImageUrl)
                        .MinimumLength(20).WithMessage("The provided ImageUrl is too short.")
                        .Must(BeAValidUrl).WithMessage("The ImageUrl must be a valid URL.");
                }
            );

            RuleFor(x => x.Address)
                .NotNull()
                .SetValidator(new AddressDTOValidator());
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
