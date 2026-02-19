using Application.Common;
using Application.DTO.Booking;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Booking.CreateBooking
{
    public class CreateBookingCommand : IRequest<Result<BookingDTO>>
    {
        public int EventId { get; set; }
        public int NumberOfSeats { get; set; }
        public string? Notes { get; set; }

        [JsonIgnore]
        public string? ClientId { get; set; }
    }
}