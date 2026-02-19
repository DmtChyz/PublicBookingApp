    using Application.Common;
    using Application.Interfaces;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    namespace Application.Commands.Booking.DeleteBooking
    {
        public class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, Result<bool>>
        {
            private readonly IBookingService _bookingService;

            public DeleteBookingCommandHandler(IBookingService bookingService)
            {
                _bookingService = bookingService;
            }

            public async Task<Result<bool>> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
            {
                var result = await _bookingService.DeleteBookingByIdAsync(request.BookingId,request.RequestorId);
                if (!result.Success)
                {
                    return Result<bool>.IsFailure(result.Errors);
                }
                return result;
            }
        }
    }