using Application.DTO.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Admin
{
    public class AdminEventDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int MaxAttendees { get; set; }
        public string OwnerID { get; set; }
        public bool IsCancelled { get; set; }
        public string ImageUrl { get; set; }

        public AddressDTO Address { get; set; }
    }
}
