using FluentValidation;
using FluentValidation.Results;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event;

public class EventValidatorBehavior : IEventValidatorBehavior
{
    private readonly IValidator<Models.Event> _validator;

    public EventValidatorBehavior(IValidator<Models.Event> validator)
    {
        _validator = validator;
    }
    public ValidationResult Validate(Models.Event eventData)
    { 
        var validationResult = _validator.Validate(eventData);

        var uniqueErrors = validationResult.Errors.GroupBy(x => x.PropertyName).Select(x => x.First()).ToList();
        validationResult.Errors = uniqueErrors;

        return  validationResult;
    }
}