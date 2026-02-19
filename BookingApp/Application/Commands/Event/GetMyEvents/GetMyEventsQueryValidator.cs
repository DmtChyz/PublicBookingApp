using FluentValidation;

namespace Application.Queries.Event.GetMyEvents
{
    public class GetMyEventsQueryValidator : AbstractValidator<GetMyEventsQuery>
    {
        public GetMyEventsQueryValidator()
        {
            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("Owner ID is required to fetch user's events.");
        }
    }
}