using Domain.Enums;

namespace Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public string ClientId { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
        public int NumberOfSeats { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public BookingStatus Status { get; set; }
        public string? Notes { get; set; }

        public bool IsCancelled { get ; set; }
    }
}