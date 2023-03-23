using FluentValidation.Results;

namespace SenseTowerEventAPI.Interfaces;

public interface IEventValidatorBehavior
{
    ValidationResult Validate(Models.Event eventData);
}