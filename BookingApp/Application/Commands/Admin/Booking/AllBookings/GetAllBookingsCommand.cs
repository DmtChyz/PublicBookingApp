using Application.Common;
using Application.DTO.Admin;
using Application.DTO.Booking;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Booking.AllBookings
{
    public class GetAllBookingsCommand : IRequest<Result<List<AdminBookingDTO>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
