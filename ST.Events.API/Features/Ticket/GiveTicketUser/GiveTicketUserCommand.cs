using ST.Events.API.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
// ReSharper disable UnusedMember.Global

namespace ST.Events.API.Features.Ticket.GiveTicketUser;

/// <summary>
/// Модель команды выдачи билета пользователю на мероприятие
/// </summary>
[SwaggerSchema("Модель команды выдачи билета пользователю на мероприятие")]
public class GiveTicketUserCommand : ICommand<Guid>
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
    /// Место мероприятия
    /// </summary>
    public int PlaceNumber { get; set; }
    /// <summary>
    /// Стоимость билета
    /// </summary>
    public decimal Price { get; set; }
}