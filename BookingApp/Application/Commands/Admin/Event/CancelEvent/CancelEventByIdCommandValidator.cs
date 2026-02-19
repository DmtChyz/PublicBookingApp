using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Event.CancelEvent
{
    public class CancelEventByIdCommandValidator : AbstractValidator<CancelEventByIdCommand>
    {
        public CancelEventByIdCommandValidator()
        {
            RuleFor(x => x.EventId).GreaterThan(0).WithMessage("Number must be greater than 0");
        }
    }
}
