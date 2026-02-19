using Application.Commands.Booking.CreateBooking;
using Application.Commands.Booking.DeleteBooking;
using Application.Commands.Booking.GetBookingById;
using Application.Commands.Booking.UpdateBooking;
using Application.Common;
using Application.DTO.Booking;
using Application.Parameters.Booking;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<Result<BookingDTO>> CreateBookingAsync(CreateBookingParameters command);
        Task<Result<BookingDTO>> UpdateBookingAsync(UpdateBookingParameters command);
        Task<Result<BookingDTO>> GetBookingByIdAsync(int bookingId,string requestorId);
        Task<Result<bool>> DeleteBookingByIdAsync(int bookingId,string requestorId);
        Task<Result<List<BookingDTO>>> GetBookingsByUserIdAsync(string userId);
    }
}