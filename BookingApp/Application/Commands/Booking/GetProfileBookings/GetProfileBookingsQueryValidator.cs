using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Booking.GetProfileBookings
{
    public class GetProfileBookingsQueryValidator : AbstractValidator<GetProfileBookingsQuery>
    {
        public GetProfileBookingsQueryValidator()
        {
            RuleFor(x => x.UserId).NotNull().WithMessage("Cannot be null");
        }
    }
}
