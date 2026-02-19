using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Event
{
    public class UpdateEventDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDate { get; set; }
        public int? MaxAttendees { get; set; }

        public string? ImageUrl { get; set; }
        public UpdateAddressDTO? Address { get; set; }
    }
}
