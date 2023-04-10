using FluentValidation;
using FluentValidation.Results;
using ST.Events.API.Interfaces;

namespace ST.Events.API.Features.Event.EventCreate;

public class EventCreateValidatorBehavior : IEventCreateValidatorBehavior
{
    private readonly IValidator<EventCreateCommand> _validator;

    public EventCreateValidatorBehavior(IValidator<EventCreateCommand> validator)
    {
        _validator = validator;
    }
    public ValidationResult Validate(EventCreateCommand eventCreateCmd)
    {
        var validationResult = _validator.Validate(eventCreateCmd);

        var uniqueErrors = validationResult.Errors.GroupBy(x => x.PropertyName).Select(x => x.First()).ToList();
        validationResult.Errors = uniqueErrors;

        return validationResult;
    }
}