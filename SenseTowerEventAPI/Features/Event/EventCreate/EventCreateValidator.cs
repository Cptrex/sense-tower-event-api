using FluentValidation;
using SenseTowerEventAPI.Interfaces;
#pragma warning disable CA1847

namespace SenseTowerEventAPI.Features.Event.EventCreate;

public class EventCreateValidator : AbstractValidator<EventCreateCommand>
{
    public EventCreateValidator(IEventValidatorManager _eventValidatorRepository)
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

        RuleFor(e => e.ImageId).NotNull().Must(_eventValidatorRepository.IsImageIdExist).WithMessage("Такого изображения не существует");
        RuleFor(e => e.SpaceId).NotNull().Must(_eventValidatorRepository.IsSpaceIdExist).WithMessage("Такого пространства не существует");
    }
}