using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Event
{
    public class GenerateDescriptionDTO
    {
        public string? Title { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public decimal? Price { get; set; }

        public string UserPrompt { get; set; }
    }
}
