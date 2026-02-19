using FluentValidation;
using Application.Commands.Admin.Booking.GetBookingById;

namespace Application.Commands.Admin.Booking
{
    public class GetBookingByIdCommandValidator : AbstractValidator<GetBookingByIdCommand>
    {
        public GetBookingByIdCommandValidator()
        {
            RuleFor(x => x.BookingId).NotEmpty()
                .WithMessage("BookingId cannot be empty");
        }
    }
}
