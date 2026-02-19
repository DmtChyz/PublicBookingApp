using Application.Common;
using Application.DTO.Booking;
using Application.DTO.Event;
using Domain.Interfaces;
using MediatR;
using Application.Interfaces;
using Application.Parameters.Booking;

namespace Application.Commands.Booking.UpdateBooking
{
    public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand,Result<BookingDTO>>
    {
        private readonly IBookingService _bookingService;

        public UpdateBookingCommandHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<Result<BookingDTO>> Handle(UpdateBookingCommand command,CancellationToken token)
        {
            var parameters = new UpdateBookingParameters
            {
                BookingId = command.BookingId,
                ClientId = command.ClientId,
                Notes = command.Notes,
                NumberOfSeats = command.NumberOfSeats,
            };
            var updateResult = await _bookingService.UpdateBookingAsync(parameters);

            if (!updateResult.Success)
            {
                return Result<BookingDTO>.IsFailure(updateResult.Errors);
            }

            return Result<BookingDTO>.IsSuccess(updateResult.Value);
        }
    }
}
