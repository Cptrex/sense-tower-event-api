using MediatR;
using SenseTowerEventAPI.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SenseTowerEventAPI.Features.Event.EventDelete;

/// <summary>
/// Модель обработчика команды удаления мероприятия
/// </summary>
[SwaggerSchema("Модель обработчика команды удаления мероприятия")]
public class EventDeleteCommand : IRequest<Guid>, IEntity
{
    public Guid ID { get; set; }
}