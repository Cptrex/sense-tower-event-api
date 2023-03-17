using MediatR;
using SenseTowerEventAPI.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SenseTowerEventAPI.Features.Ticket.GiveTicketUser;

/// <summary>
/// Модель команды выдачи билета пользователю на мероприятие
/// </summary>
[SwaggerSchema("Модель команды выдачи билета пользователю на мероприятие")]
public class GiveTicketUserCommand : IRequest<Guid>, ITicket
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

    public GiveTicketUserCommand(Guid iD, string owner, string? place)
    {
        ID = iD;
        Owner = owner;
        Place = place;
    }
}