using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<Booking?> GetByIdWithEventAsync(int bookingId);
        Task<IQueryable<Booking>> GetQueryableWithEventAsync();
    }
}
