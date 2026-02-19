using Application.Common;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Booking.DeleteBooking
{
    public class DeleteBookingCommand : IRequest<Result<bool>>
    {
        [JsonIgnore]
        public int BookingId { get; set; }

        [JsonIgnore]
        public string RequestorId { get; set; }
    }
}