using JetBrains.Annotations;
using MediatR;

namespace SenseTowerEventAPI.Features.Ticket.CheckTicketUserExist;

[UsedImplicitly]
public class CheckTicketUserExistHandler : IRequestHandler<CheckTicketUserExistQuery, Guid>
{
    public Task<Guid> Handle(CheckTicketUserExistQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}