using Application.Common;
using Application.DTO.Booking;
using Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.Booking.GetBookingById
{
    public class GetBookingByIdCommandHandler : IRequestHandler<GetBookingByIdCommand, Result<BookingDTO>>
    {
        private readonly IBookingService _bookingService;

        public GetBookingByIdCommandHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<Result<BookingDTO>> Handle(GetBookingByIdCommand request, CancellationToken cancellationToken)
        {
            var bookingResult = await _bookingService.GetBookingByIdAsync(request.BookingId,request.RequestorId);
            if (!bookingResult.Success)
            {
                return Result<BookingDTO>.IsFailure(bookingResult.Errors);
            }
            return bookingResult;
        }
    }
}