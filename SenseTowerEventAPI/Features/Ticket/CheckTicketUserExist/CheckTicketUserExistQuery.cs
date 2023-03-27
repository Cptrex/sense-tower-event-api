using MediatR;

namespace SenseTowerEventAPI.Features.Ticket.CheckTicketUserExist;
/// <summary>
/// Модель запроса наличия билета у пользователя
/// </summary>
public class CheckTicketUserExistQuery : IRequest<bool>
{
    /// <summary>
    /// Уникальный идентификатор мероприятия
    /// </summary>
    public Guid EventId { get; set; }
    /// <summary>
    /// Уникальный идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Уникальный идентификатор билета
    /// </summary>
    public Guid TicketId { get; set; }
}