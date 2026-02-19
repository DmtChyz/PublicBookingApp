using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Event.GetAllEvents
{
    public class GetAllEventsCommandValidator : AbstractValidator<GetAllEventsCommand> 
    {
        public GetAllEventsCommandValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1");

            RuleFor(x => x.PageSize)
                    .GreaterThan(0).WithMessage("Page size must be greater than 0.");
        }
    }
}
