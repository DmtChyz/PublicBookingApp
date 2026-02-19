using Application.Common;
using Application.DTO.Admin;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Commands.Admin.Booking.GetBookingById
{
    public class GetBookingByIdCommand : IRequest<Result<AdminBookingDTO>>
    {
        [JsonIgnore]
        public int BookingId { get; set; }
    }
}