using MediatR;

namespace SenseTowerEventAPI.Features.Ticket.CheckTicketUserExist;

public class CheckTicketUserExistQuery : IRequest<bool>
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public Guid TicketId { get; set; }
}