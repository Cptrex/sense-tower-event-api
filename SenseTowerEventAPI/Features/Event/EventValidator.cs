using FluentValidation;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event
{
    public class EventValidator : AbstractValidator<IEvent>
    {

        private readonly IEventSingleton _eventInstance;
        
        public EventValidator(IEventSingleton eventSingleton) 
        {
            _eventInstance = eventSingleton;

            RuleFor(e => e.EndDate)
                .NotNull().NotEmpty().WithMessage("EndDate не может быть пустым");

            RuleFor(e=> e.StartDate)
                .NotNull().NotEmpty().WithMessage("StartDate не может быть пустым")
                .LessThan(d => d.EndDate).WithMessage("StartDate не может быть больше, чем EndDate");

            RuleFor(e => e.Title).NotNull().MinimumLength(2).MaximumLength(100);
            RuleFor(e => e.Description).MaximumLength(500);

            RuleFor(e => e.ImageID).Must(e=> IsImageIDExist(e) == true).WithMessage("Такого изображения не существует");
            RuleFor(e => e.SpaceID).NotNull().Must(e=> IsSpaceIDExist(e) == true).WithMessage("Такого пространства не существует");
        }

        public bool IsImageIDExist(Guid imageID)
        {

            return _eventInstance.Images.Contains(imageID);
        }

        public bool IsSpaceIDExist(Guid spaceID)
        {
            return _eventInstance.Spaces.Contains(spaceID);

        }
    }
}