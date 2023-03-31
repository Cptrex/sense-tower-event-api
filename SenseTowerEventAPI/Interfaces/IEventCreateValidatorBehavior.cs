using FluentValidation.Results;
using SenseTowerEventAPI.Features.Event.EventCreate;

namespace SenseTowerEventAPI.Interfaces;

public interface IEventCreateValidatorBehavior
{
    ValidationResult Validate(EventCreateCommand cmdModel);
}