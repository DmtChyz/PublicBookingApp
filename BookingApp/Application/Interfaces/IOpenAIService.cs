using Application.Common;
using Application.DTO.Event;
using Application.DTO.Validation;
namespace Application.Interfaces
{
    public interface IOpenAIService
    {
        Task<Result<string>> GenerateEventDescriptionAsync(GenerateDescriptionDTO request);
        Task<Result<bool>> ValidatePromptSafetyAsync(PromptModerationRequestPayload data);
        Task<Result<bool>> ValidatePromptRelevanceAsync(PromptRelevanceRequestPayload data);
    }
}
