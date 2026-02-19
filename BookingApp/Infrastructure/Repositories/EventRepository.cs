using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    // context from generic
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Booking> GetByIdWithEventAsync(int bookingId)
        {
            return await _context.Bookings
                                 .Include(b => b.Event)
                                 .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<Event?> GetByIdWithBookingsAsync(int eventId)
        {
            return await _context.Events
                .Include(e => e.Bookings) // to count for available seats
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Event, bool>> predicate)
        {
            return await _context.Set<Event>().AnyAsync(predicate);
        }

        public async Task<Event?> GetByTitleAsync(string title)
        {
            return await _context.Events
                .FirstOrDefaultAsync(e => e.Title == title);
        }
    }
}
