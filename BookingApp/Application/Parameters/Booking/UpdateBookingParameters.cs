using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Parameters.Booking
{
    public class UpdateBookingParameters
    {
        public int BookingId { get; set; }
        public int NumberOfSeats { get; set; }
        public string? Notes { get; set; }
        public string? ClientId { get; set; }
    }
}
