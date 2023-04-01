using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SC.Internship.Common.Exceptions;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.MongoDB.Context;

namespace SenseTowerEventAPI.Features.Ticket.CheckTicketUserExist;

[UsedImplicitly]
public class CheckTicketUserExistHandler : IRequestHandler<CheckTicketUserExistQuery, bool>
{
    private readonly IMongoCollection<Models.Event> _eventContext;
    private readonly IEventSingleton _eventInstance;

    public CheckTicketUserExistHandler(IEventSingleton eventInstance, IOptions<EventContext> options)
    {
        _eventInstance = eventInstance;
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _eventContext = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Models.Event>(options.Value.CollectionName);
    }

    public async Task<bool> Handle(CheckTicketUserExistQuery request, CancellationToken cancellationToken)
    {
        var foundUser = _eventInstance.Users.FirstOrDefault(u => u.Id == request.UserId);
        
        if (foundUser == null) throw new ScException("Пользователь не найден");

        var foundEvent = (await _eventContext.Find(e=> e.Id == request.EventId).ToListAsync(cancellationToken)).First();

        var foundTicketUser = foundEvent.Tickets.FirstOrDefault(t => t.Id == request.TicketId);

        if (foundTicketUser == null) throw new ScException("Билет не найден");

        return true;
    }
}