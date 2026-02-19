using Domain.Interfaces;
using FluentValidation;


namespace Application.Commands.Event.UpdateEvent
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        private readonly IEventRepository _eventRepository;

        public UpdateEventCommandValidator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;

            RuleFor(x => x.Id)
                .NotEmpty()
                .MustAsync(async (id, cancellationToken) => await _eventRepository.GetByIdAsync(id) != null)
                .WithMessage("Event with the specified ID was not found.");

            RuleFor(x => x.Title)
            .MinimumLength(8).WithMessage("Event title must be at least 8 characters long.")
            .MaximumLength(80).WithMessage("Event title must not exceed 80 characters.")
            .Matches(@"^[\p{L}\p{N}_ '.-]+$").WithMessage("Event title can only contain letters, numbers, underscore, and hyphen.")
            .MustAsync(async (command, title, cancellationToken) =>
            {
                return !await _eventRepository.ExistsAsync(e => e.Title == title && e.Id != command.Id);
            })
            .WithMessage("An event with this title already exists.")
            .When(x => x.Title != null);

            RuleFor(x => x.Description)
                .MinimumLength(24).WithMessage("Description must be at least 24 characters long.")
                .MaximumLength(512).WithMessage("Description must not exceed 512 characters.")
                .When(x => x.Description != null);

            RuleFor(x => x.EventDate)
                .Must(date => date > DateTime.UtcNow.AddDays(1)).WithMessage("Event date must be in the future (at least tomorrow).")
                .When(x => x.EventDate.HasValue);

            RuleFor(x => x.MaxAttendees)
                .GreaterThan(0).WithMessage("Max attendees must be greater than 0.")
                .When(x => x.MaxAttendees.HasValue);
        }
    }
}