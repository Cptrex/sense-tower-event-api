using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace SenseTowerEventAPI.Features.Event.EventDelete;

/// <summary>
/// Модель обработчика команды удаления мероприятия
/// </summary>
[SwaggerSchema("Модель обработчика команды удаления мероприятия")]
public class EventDeleteCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
}