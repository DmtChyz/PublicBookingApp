using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin.Booking.CancelBookingById
{
    public class CancelBookingByIdCommand : IRequest<Result<bool>>  
    {
        public int BookingId { get; set; }  
    }
}
