using FluentValidation;

namespace Application.Queries.Event.GetAllPublicEvents
{
    public class GetAllPublicEventsQueryValidator : AbstractValidator<GetAllPublicEventsQuery>
    {
        public GetAllPublicEventsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than zero.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than zero.")
                .LessThanOrEqualTo(100)
                .WithMessage("Page size cannot be greater than 100.");
        }
    }
}