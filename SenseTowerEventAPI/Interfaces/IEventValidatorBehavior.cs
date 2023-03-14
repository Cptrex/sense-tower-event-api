using FluentValidation.Results;

namespace SenseTowerEventAPI.Interfaces
{
    public interface IEventValidatorBehavior
    {
        Task<ValidationResult> Validate(IEvent ievent);
    }
}