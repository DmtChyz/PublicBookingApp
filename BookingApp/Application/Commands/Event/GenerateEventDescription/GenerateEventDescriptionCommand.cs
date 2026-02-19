using Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Event.GenerateEventDescription
{
    public class GenerateEventDescriptionCommand : IRequest<Result<string>>
    {
        public string Title { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        [Required]
        public string UserPrompt { get; set; }
    }
}
