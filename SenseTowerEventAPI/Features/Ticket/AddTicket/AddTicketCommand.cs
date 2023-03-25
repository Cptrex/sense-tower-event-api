using JetBrains.Annotations;
using MediatR;

namespace SenseTowerEventAPI.Features.Ticket.AddTicket;

/// <summary>
/// Модель команды добавления бесплатного билета
/// </summary>
[UsedImplicitly]
public class AddTicketCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid Owner { get; set; }
    public int PlaceNumber { get; set; }

    public AddTicketCommand(Guid id, Guid eventId, Guid owner, int place)
    {
        Id = id;
        EventId= eventId;
        Owner = owner;
        PlaceNumber = place;
    }
}