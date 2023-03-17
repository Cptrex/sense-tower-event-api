using JetBrains.Annotations;
using MediatR;
using SC.Internship.Common.Exceptions;
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
        var foundUser = _eventInstance.Users.FirstOrDefault(u => u.Id == request.Owner);
        if (foundUser == null) throw new ScException("Не удалось выдать билет. Пользователь не найден");

        foundUser.Tickets.Add(request);

        return await Task.FromResult(request.Owner);
    }
}