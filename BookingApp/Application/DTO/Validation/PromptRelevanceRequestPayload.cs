using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Validation
{
    public class PromptRelevanceRequestPayload
    {
        public string UserPrompt { get; set; }
        public string RelevanceContext { get; set; }
    }
}
