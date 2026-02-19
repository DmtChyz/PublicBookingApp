using Application.Common;
using Application.DTO.Booking;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commands.Booking.GetProfileBookings
{
    public class GetProfileBookingsQuery : IRequest<Result<List<BookingDTO>>>
    {
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
