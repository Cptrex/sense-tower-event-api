using FluentValidation;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event;

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

        RuleFor(e => e.Title).NotNull();
        RuleFor(e => e.Title).MinimumLength(2);
        RuleFor(e => e.Title).MaximumLength(5);
        RuleFor(e => e.Title).Must(e=> e.Contains("t")).WithMessage("Заголовок должен содержать t");

        RuleFor(e => e.Description).NotNull();
        RuleFor(e => e.Description).MinimumLength(2);
        RuleFor(e => e.Description).MaximumLength(5);
        RuleFor(e => e.Description).Must(e => e.Contains("d")).WithMessage("Описание должно содержать d");
        // RuleFor(e => e.Title).NotNull().MinimumLength(2).MaximumLength(5);
        //RuleFor(e => e.Description).NotNull().MaximumLength(1);

        RuleFor(e => e.ImageId).NotNull().Must(IsImageIdExist).WithMessage("Такого изображения не существует");
        RuleFor(e => e.SpaceId).NotNull().Must(IsSpaceIdExist).WithMessage("Такого пространства не существует");
    }

    public bool IsImageIdExist(Guid imageId)
    {

        return _eventInstance.Images.Contains(imageId);
    }

    public bool IsSpaceIdExist(Guid spaceId)
    {
        return _eventInstance.Spaces.Contains(spaceId);

    }
}