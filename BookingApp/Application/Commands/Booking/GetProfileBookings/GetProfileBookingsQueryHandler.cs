using Application.Common;
using Application.DTO.Booking;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Booking.GetProfileBookings
{
    public class GetProfileBookingsQueryHandler : IRequestHandler<GetProfileBookingsQuery, Result<List<BookingDTO>>>
    {
        private readonly IBookingService _bookingService;
        public GetProfileBookingsQueryHandler(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<Result<List<BookingDTO>>> Handle(GetProfileBookingsQuery request, CancellationToken cancellationToken)
        {   // check if is Success is moved to the controller
            return await _bookingService.GetBookingsByUserIdAsync(request.UserId);
        }
    }
}
