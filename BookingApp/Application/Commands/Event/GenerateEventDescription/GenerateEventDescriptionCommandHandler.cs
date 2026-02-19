using Application.Common;
using Application.DTO.Event;
using Application.DTO.Validation;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Event.GenerateEventDescription
{
    public class GenerateEventDescriptionCommandHandler : IRequestHandler<GenerateEventDescriptionCommand, Result<string>>
    {
        private readonly IOpenAIService _openAIService;
        public GenerateEventDescriptionCommandHandler(IOpenAIService openAiService)
        {
            _openAIService = openAiService;
        }

        public async Task<Result<string>> Handle(GenerateEventDescriptionCommand request, CancellationToken cancellationToken)
        {
            var userValidationData = new PromptModerationRequestPayload()
            {
                UserPrompt = request.UserPrompt
            };

            var isFlagged = await _openAIService.ValidatePromptSafetyAsync(userValidationData);
            if(!isFlagged.Value) return Result<string>.IsFailure("Illegal content. Please try again.");

            var userRelevanceData = new PromptRelevanceRequestPayload()
            {
                UserPrompt = userValidationData.UserPrompt,
                RelevanceContext = "will be used in Event description generation"
            };
            var isRellevant = await _openAIService.ValidatePromptRelevanceAsync(userRelevanceData);
            if (!isRellevant.Value) return Result<string>.IsFailure("Whoops, looks like your message is not rellevant. Try explain it better.");

            var generateDescriptionDTO = new GenerateDescriptionDTO()
            {
                City = request.City,
                Price = request.Price,
                Country = request.Country,
                Title = request.Title,
                UserPrompt = request.UserPrompt,
            };
            var result = await _openAIService.GenerateEventDescriptionAsync(generateDescriptionDTO);
            return result;
        }
    }
}
