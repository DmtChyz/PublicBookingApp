using Application.DTO.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Parameters.Events
{
    public class UpdateEventParameters
    {
        public int Id { get; set; }
        public string RequestorID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDate { get; set; }
        public int? MaxAttendees { get; set; }
        public string? ImageUrl { get; set; }
        public UpdateAddressDTO? Address { get; set; }
    }
}
