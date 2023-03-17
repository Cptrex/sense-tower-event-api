using JetBrains.Annotations;
using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Ticket.GiveTicketUser;

[UsedImplicitly]
public class GiveTicketUserHandler : IRequestHandler<GiveTicketUserCommand, Guid>
{
    private readonly IEventSingleton _eventInstance;

    public GiveTicketUserHandler(IEventSingleton eventInstance)
    {
        _eventInstance = eventInstance;
    }
        
    public async Task<Guid> Handle(GiveTicketUserCommand request, CancellationToken cancellationToken)
    {
        _eventInstance.Events.Clear();
        return await Task.FromResult(Guid.NewGuid());
    }
}