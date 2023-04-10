using ST.Events.API.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ST.Events.API.Features.Event.EventDelete;

/// <summary>
/// Модель обработчика команды удаления мероприятия
/// </summary>
[SwaggerSchema("Модель обработчика команды удаления мероприятия")]
public class EventDeleteCommand : ICommand<Guid>
{
    public Guid Id { get; set; }
}