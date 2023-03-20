using MongoDB.Bson.Serialization.Attributes;
using SenseTowerEventAPI.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using MongoDB.Bson;

namespace SenseTowerEventAPI.Models;

/// <summary>
/// Модель билетов
/// </summary>
[SwaggerSchema("Модель билетов")]
public class Ticket : ITicket, IEntity
{
    /// <summary>
    /// Уникальный идентификатор билета
    /// </summary>
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    /// <summary>
    /// Уникальный идентификатор мероприятия
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    [BsonElement("elementId")]
    public Guid EventId { get; set; }

    /// <summary>
    /// Владелец билета
    /// </summary>
    [BsonRepresentation(BsonType.String)]
    [BsonElement("owner")]
    public Guid Owner { get; set; }

    /// <summary>
    /// Номер места на мероприятии
    /// </summary>
    [BsonRepresentation(BsonType.Int32)]
    [BsonElement("placeNumber")]
    public int PlaceNumber { get; set; }

    public Ticket(Guid id, Guid eventId, Guid owner, int placeNumber)
    {
        Id = id;
        EventId = eventId;
        Owner = owner;
        PlaceNumber = placeNumber;
    }
}