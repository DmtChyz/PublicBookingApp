using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        public Task<Event?> GetByTitleAsync(string title);
        Task<bool> ExistsAsync(Expression<Func<Event, bool>> predicate);
        Task<Event?> GetByIdWithBookingsAsync(int eventId);
    }
}
