using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Event.GetEventById
{
    public class GetEventByIdCommandValidator : AbstractValidator<GetEventByIdCommand>
    {
        public GetEventByIdCommandValidator()
        {
            RuleFor(x => x.EventId).GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
