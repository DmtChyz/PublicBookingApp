using Application.Common;
using Application.DTO.Admin;

namespace Application.Interfaces
{
    public interface IAdminService
    {
        Task<Result<List<AdminEventDTO>>> GetAllEventsAsync(int page,int pageSize);

        Task<Result<AdminEventDTO>> GetEventByIdAsync(int eventId);

        Task<Result<bool>> CancelEventAsync(int eventId);

        Task<Result<List<AdminBookingDTO>>> GetAllBookingsAsync(int page,int pageSize);

        Task<Result<AdminBookingDTO>> GetBookingByIdAsync(int bookingId);

        Task<Result<bool>> CancelBookingAsync(int bookingId);

        Task<Result<List<UserDTO>>> GetAllUsersAsync(int page,int pageSize);

        Task<Result<UserDTO>> GetUserByIdAsync(string userId);
    }
}
