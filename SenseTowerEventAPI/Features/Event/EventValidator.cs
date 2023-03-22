using FluentValidation;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event;

public class EventValidator : AbstractValidator<IEvent>
{
    public EventValidator(IEventSingleton _eventInstance, IEventValidatorRepository _eventValidatorRepository)
    {
        RuleFor(e => e.EndDate)
            .NotNull().NotEmpty().WithMessage("EndDate не может быть пустым");

        RuleFor(e => e.StartDate)
            .NotNull().NotEmpty().WithMessage("StartDate не может быть пустым")
            .LessThan(d => d.EndDate).WithMessage("StartDate не может быть больше, чем EndDate");

        RuleFor(e => e.Title).NotNull();
        RuleFor(e => e.Title).MinimumLength(1);
        RuleFor(e => e.Title).MaximumLength(10);
        RuleFor(e => e.Title).Must(e => e.Contains("s")).WithMessage("Заголовок должен содержать s");

        RuleFor(e => e.Description).NotNull();
        RuleFor(e => e.Description).MinimumLength(1);
        RuleFor(e => e.Description).MaximumLength(10);
        RuleFor(e => e.Description).Must(e => e.Contains("s")).WithMessage("Описание должно содержать s");


        RuleFor(e => e.ImageId).NotNull().Must(e => _eventValidatorRepository.IsImageIdExist(_eventInstance, e)).WithMessage("Такого изображения не существует");
        RuleFor(e => e.SpaceId).NotNull().Must(e => _eventValidatorRepository.IsSpaceIdExist(_eventInstance, e)).WithMessage("Такого пространства не существует");

    }
}