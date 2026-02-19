using Application.Common;
using Application.DTO.Booking;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Booking.GetBookingById
{
    public class GetBookingByIdCommand : IRequest<Result<BookingDTO>>
    {
        [JsonIgnore]
        public int BookingId { get; set; }

        [JsonIgnore]
        public string RequestorId { get; set; }
    }
}