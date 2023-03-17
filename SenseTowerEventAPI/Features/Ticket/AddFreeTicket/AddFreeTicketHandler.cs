using JetBrains.Annotations;
using MediatR;
using SenseTowerEventAPI.Extensions;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Ticket.AddFreeTicket;

[UsedImplicitly]
public class AddFreeTicketHandler :IRequestHandler<AddFreeTicketCommand, Guid>
{
    private readonly IEventSingleton _eventInstance;

    public AddFreeTicketHandler(IEventSingleton eventInstance)
    {
        _eventInstance = eventInstance;
    }

    public async Task<Guid> Handle(AddFreeTicketCommand request, CancellationToken cancellationToken)
    {
        var foundEvent = _eventInstance.Events.FirstOrDefault(e => e.Id == request.EventId);
        if (foundEvent == null) throw new StException("Ошибка добавления билета. Мероприятие не найдено");

        var ticket = new Models.Ticket(Guid.NewGuid(), foundEvent.Id, request.Owner, request.Place);
        foundEvent.Tickets.Add(ticket);

        return await Task.FromResult(ticket.Id);
    }
}