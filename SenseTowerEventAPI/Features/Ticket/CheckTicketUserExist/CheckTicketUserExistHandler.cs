using JetBrains.Annotations;
using MediatR;
using SenseTowerEventAPI.Extensions;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Ticket.CheckTicketUserExist;

[UsedImplicitly]
public class CheckTicketUserExistHandler : IRequestHandler<CheckTicketUserExistQuery, bool>
{
    private readonly IEventSingleton _eventInstance;

    public CheckTicketUserExistHandler(IEventSingleton eventInstance)
    {
        _eventInstance = eventInstance;
    }

    public async Task<bool> Handle(CheckTicketUserExistQuery request, CancellationToken cancellationToken)
    {
        var foundUser = _eventInstance.Users.FirstOrDefault(u => u.Id == request.UserId);

        if (foundUser == null) throw new StException("Пользователь не найден");

        var foundTicketUser = foundUser.Tickets.FirstOrDefault(t => t.EventId == request.EventId && t.Id == request.TicketId);
        
        if(foundTicketUser == null) return await Task.FromResult(false);
        
        return await Task.FromResult(true);
    }
}