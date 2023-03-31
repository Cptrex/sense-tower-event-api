using JetBrains.Annotations;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;
#pragma warning disable CS8618

namespace SenseTowerEventAPI.Features.Event.EventCreate;

/// <summary>
/// Модель команды создания мероприятия
/// </summary>
[SwaggerSchema("Модель команды создания мероприятия")]
public class EventCreateCommand : IRequest<Guid>
{
    /// <summary>
    /// Уникальный идентификатор мероприятия
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    /// <summary>
    /// Название мероприятия
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Дата начала меропрития
    /// </summary>
    public DateTimeOffset StartDate { get; set; }
    /// <summary>
    /// Дата завершения мероприятия
    /// </summary>
    public DateTimeOffset EndDate { get; set; }
    /// <summary>
    /// Описание мероприятия
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Уникальный идентификатор изображения мероприятия
    /// </summary>
    public Guid ImageId { get; set ; }
    /// <summary>
    /// Уникальный идентификатор пространства мероприятия
    /// </summary>
    public Guid SpaceId { get; set ; }

    /// <summary>
    /// Список билетов мероприятия
    /// </summary>
    [UsedImplicitly]
    public List<Models.Ticket> Tickets { get; set; }
}