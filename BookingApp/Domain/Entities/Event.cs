using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int MaxAttendees { get; set; }
        public decimal Price { get; set; }

        public List<Booking> Bookings { get; set; } = new List<Booking>();

        public string CreatedByUserId { get; set; }

        public string ImageUrl { get; set; } = "https://placehold.co/600x400";

        public bool IsCancelled { get; set; }

        public Address Address { get; set; }
    }
}