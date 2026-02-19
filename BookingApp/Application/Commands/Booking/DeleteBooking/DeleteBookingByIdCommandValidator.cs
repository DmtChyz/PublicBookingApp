using FluentValidation;

namespace Application.Commands.Booking.DeleteBooking
{
    public class DeleteBookingCommandValidator : AbstractValidator<DeleteBookingCommand>
    {
        public DeleteBookingCommandValidator()
        {
            RuleFor(x => x.BookingId)
                .GreaterThan(0).WithMessage("Booking ID must be a positive number.");

            RuleFor(x => x.RequestorId)
                .NotEmpty().WithMessage("Requestor ID is required.");
        }
    }
}