using FluentValidation;

namespace Application.Commands.Booking.GetBookingById
{
    public class GetBookingByIdCommandValidator : AbstractValidator<GetBookingByIdCommand>
    {
        public GetBookingByIdCommandValidator()
        {
            RuleFor(x => x.BookingId)
                .GreaterThan(0).WithMessage("Booking ID must be a positive number.");

            RuleFor(x => x.RequestorId)
                .NotEmpty().WithMessage("Requestor ID is required.");
        }
    }
}