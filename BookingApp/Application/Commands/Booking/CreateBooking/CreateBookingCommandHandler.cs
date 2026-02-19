using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Application.Common;
using Application.Interfaces;
using Application.Exceptions;
using Application.DTO.Booking;
using Application.Parameters.Booking;

namespace Application.Commands.Booking.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<BookingDTO>>
    {
        private readonly IBookingService _bookingService;

        public CreateBookingCommandHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<Result<BookingDTO>> Handle(CreateBookingCommand request, CancellationToken token)
        {
            var parameters = new CreateBookingParameters
            {
                ClientId = request.ClientId,
                EventId = request.EventId,
                Notes = request.Notes,
                NumberOfSeats = request.NumberOfSeats,
            };

            var isSuccessfullyAdded = await _bookingService.CreateBookingAsync(parameters);

            if (!isSuccessfullyAdded.Success)
            {
                return Result<BookingDTO>.IsFailure(isSuccessfullyAdded.Errors);
            }

            return isSuccessfullyAdded;
        }
    }
}
