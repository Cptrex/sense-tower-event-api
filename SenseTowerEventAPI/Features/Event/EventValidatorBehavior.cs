using FluentValidation;
using FluentValidation.Results;
using SenseTowerEventAPI.Interfaces;
using System;

namespace SenseTowerEventAPI.Features.Event
{
    public class EventValidatorBehavior : IEventValidatorBehavior
    {
        private readonly IValidator<IEvent> _validator;

        public EventValidatorBehavior(IValidator<IEvent> validator)
        {
            _validator = validator;
        }
        public async Task<ValidationResult> Validate(IEvent eventData)
        {
            return await _validator.ValidateAsync(eventData);

        }
    }
}
