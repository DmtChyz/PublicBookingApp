using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class BookingRepository : GenericRepository<Booking> , IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context) 
        {

        }

        public async Task<Booking?> GetByIdWithEventAsync(int bookingId)
        {
            return await _context.Bookings
                                 .Include(b => b.Event)
                                 .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public Task<IQueryable<Booking>> GetQueryableWithEventAsync()
        {
            var query = _context.Bookings.Include(b => b.Event).AsNoTracking();
            return Task.FromResult(query);
        }
    }
}
