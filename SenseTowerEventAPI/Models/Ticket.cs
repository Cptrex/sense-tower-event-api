using Swashbuckle.AspNetCore.Annotations;

namespace SenseTowerEventAPI.Models;

/// <summary>
/// Модель билетов
/// </summary>
[SwaggerSchema("Модель билетов")]
public class Ticket
{
    /// <summary>
    /// Уникальный идентификатор билета
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Уникальный идентификатор мероприятия
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Владелец билета
    /// </summary>
    public Guid Owner { get; set; }

    /// <summary>
    /// Номер места на мероприятии
    /// </summary>
    public int PlaceNumber { get; set; }

    /// <summary>
    /// Цена билета
    /// </summary>
    public decimal Price { get; set; }

    public Ticket(Guid id, Guid eventId, Guid owner, int placeNumber, decimal price)
    {
        Id = id;
        EventId = eventId;
        Owner = owner;
        PlaceNumber = placeNumber;
        Price  = price;
    }
}