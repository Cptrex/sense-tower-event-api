using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace SenseTowerEventAPI.Features.Ticket.GiveTicketUser;

/// <summary>
/// Модель команды выдачи билета пользователю на мероприятие
/// </summary>
[SwaggerSchema("Модель команды выдачи билета пользователю на мероприятие")]
public class GiveTicketUserCommand : IRequest<Guid>
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

    public GiveTicketUserCommand(Guid id, Guid eventId, Guid owner, int placeNumber)
    {
        Id = id;
        EventId = eventId;
        Owner = owner;
        PlaceNumber = placeNumber;
    }
}