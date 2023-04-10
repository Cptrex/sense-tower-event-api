using FluentValidation.Results;
using JetBrains.Annotations;
using ST.Events.API.Features.Event.EventCreate;

namespace ST.Events.API.Interfaces;

public interface IEventCreateValidatorBehavior
{
    [UsedImplicitly]
    ValidationResult Validate(EventCreateCommand cmdModel);
}