using FluentValidation;
using FluentValidation.Results;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event;

public class EventValidatorBehavior : IEventValidatorBehavior
{
    private readonly IValidator<IEvent> _validator;

    public EventValidatorBehavior(IValidator<IEvent> validator)
    {
        _validator = validator;
    }
    public ValidationResult Validate(IEvent eventData)
    { 
        var validationResult = _validator.Validate(eventData);

        var uniqueErrors = validationResult.Errors.GroupBy(x => x.PropertyName).Select(x => x.First()).ToList();
        validationResult.Errors = uniqueErrors;

        return  validationResult;
    }
}