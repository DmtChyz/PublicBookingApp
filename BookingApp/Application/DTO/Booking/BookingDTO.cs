using Application.DTO.Event;
using Domain.Enums;
using System;

namespace Application.DTO.Booking
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public EventSummaryDTO EventSummary { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
        public int NumberOfSeats { get; set; }
    }
}