using Application.Commands.Event.GetEventById;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Queries.Event.GetEventById
{
    public class GetEventByIdQueryValidator : AbstractValidator<GetEventByIdCommand>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventByIdQueryValidator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;

            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Event ID must be a positive number.")
                .MustAsync(async (id, cancellationToken) =>
                {
                    return await _eventRepository.GetByIdAsync(id) != null;
                })
                .WithMessage("Event with the specified ID was not found.");
        }
    }
}