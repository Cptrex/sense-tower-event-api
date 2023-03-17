using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Ticket.CheckTicketUserExist;

public class CheckTicketUserExistQuery : IRequest<bool>, IEntity
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public Guid TicketId { get; set; }
}