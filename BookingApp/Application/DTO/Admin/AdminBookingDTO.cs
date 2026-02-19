using Application.DTO.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Admin
{
    public class AdminBookingDTO
    {
        public int Id { get; set; }
        public EventSummaryDTO EventSummary { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
        public int NumberOfSeats { get; set; }
        public bool IsCancelled { get; set; }
    }
}
