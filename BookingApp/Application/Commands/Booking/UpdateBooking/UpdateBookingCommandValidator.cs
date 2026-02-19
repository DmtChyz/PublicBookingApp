using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Booking.UpdateBooking
{
    public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
    {
       public UpdateBookingCommandValidator()
        {
            RuleFor(x => x.NumberOfSeats)
               .GreaterThan(0).WithMessage("Number of seats must be greater than 0.");

            RuleFor(x => x.Notes)
                    .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
                    .When(x => !string.IsNullOrEmpty(x.Notes));


            RuleFor(x => x.BookingId)
                    .GreaterThan(0).WithMessage("Booking ID must be a positive number.");

            RuleFor(x => x.ClientId)
                    .NotEmpty().WithMessage("Client ID is required for authorization.");
        }
    }
}
