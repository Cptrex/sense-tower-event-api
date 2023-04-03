using FluentValidation.Results;
using JetBrains.Annotations;
using SenseTowerEventAPI.Features.Event.EventCreate;

namespace SenseTowerEventAPI.Interfaces;

public interface IEventCreateValidatorBehavior
{
    [UsedImplicitly]
    ValidationResult Validate(EventCreateCommand cmdModel);
}