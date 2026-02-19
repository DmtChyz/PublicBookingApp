using Application.Common;
using Domain.Entities;
using Application.DTO.Event;
using Application.Commands.Event.UpdateEvent;
using Application.Commands.Event.DeleteEvent;
using Application.Parameters.Events;

namespace Application.Interfaces
{
    public interface IEventService
    {
        public Task<Result<EventDTO>>? GetEventByNameAsync(string title, string requestorId);
        public Task<Result<EventDTO>> GetEventByIdAsync(int eventId);
        public Task<bool> IsTitleUniqueAsync(string title);
        public Task<Result<EventDTO>> CreateEventAsync(CreateEventParameters request);
        public Task<Result<EventDTO>> UpdateEventAsync(UpdateEventParameters request);
        public Task<Result<bool>> DeleteEventAsync(int eventId,string RequestorID);
        Task<Result<List<PublicEventSummaryDTO>>> GetAllPublicEventsAsync(int page, int pageSize, string? sortBy, string? sortOrder);
        Task<Result<IEnumerable<PublicEventSummaryDTO>>> GetMyEventsAsync(string ownerId);
    }
}
