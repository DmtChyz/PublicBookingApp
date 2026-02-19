using Application.Common;
using Application.DTO.Booking;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commands.Booking.UpdateBooking
{
    public class UpdateBookingCommand : IRequest<Result<BookingDTO>>
    {
        [JsonIgnore]
        public int BookingId { get; set; }

        public int NumberOfSeats { get; set; }
        public string? Notes { get; set; }

        [JsonIgnore]
        public string? ClientId { get; set; }
    }
}
