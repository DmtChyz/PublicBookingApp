using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Booking.CancelBookingById
{
    public class CancelBookingByIdCommandValidator : AbstractValidator<CancelBookingByIdCommand>
    {
        public CancelBookingByIdCommandValidator()
        {
            RuleFor(x => x.BookingId).
                GreaterThan(0).WithMessage("Booking id cannot be less or equal to zero");
        }
    }
}
