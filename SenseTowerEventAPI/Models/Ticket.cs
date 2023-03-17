using SenseTowerEventAPI.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SenseTowerEventAPI.Models;

/// <summary>
/// Модель билетов
/// </summary>
[SwaggerSchema("Модель билетов")]
public class Ticket : ITicket
{
    /// <summary>
    /// Уникальный идентификатор билета
    /// </summary>
    public Guid ID { get; set; }
    /// <summary>
    /// Владелец билета
    /// </summary>
    public string Owner { get; set; }
    /// <summary>
    /// Место мероприятия
    /// </summary>
    public string? Place { get; set; }

    public Ticket(Guid id, string owner, string place)
    {
        ID = id;
        Owner = owner;
        Place = place;
    }
}