using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SenseTowerEventAPI.Extensions;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models.Context;

namespace SenseTowerEventAPI.Features.Ticket.AddFreeTicket;

[UsedImplicitly]
public class AddFreeTicketHandler :IRequestHandler<AddFreeTicketCommand, Guid>
{
    private readonly IMongoCollection<Models.Event> _eventContext;
    private  readonly ITicketRepository _ticketRepository;

    public AddFreeTicketHandler(IOptions<EventContext> options, ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _eventContext = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Models.Event>(options.Value.CollectionName);
    }

    public async Task<Guid> Handle(AddFreeTicketCommand request, CancellationToken cancellationToken)
    {
        var foundEvent = ( await _eventContext.Find(e => e.Id == request.Id).ToListAsync(cancellationToken: cancellationToken)).FirstOrDefault();
        
        if (foundEvent == null) throw new StException("Ошибка добавления билета. Мероприятие не найдено");

        var ticketLastNumber = await _ticketRepository.GetLastTicketNumberInEvent(foundEvent.Id);
        
        var ticket = new Models.Ticket(Guid.NewGuid(),foundEvent.Id, request.Owner, ticketLastNumber + 1);
        
        foundEvent.Tickets.Add(ticket);

        var filter = Builders<Models.Event>.Filter.Eq("id", foundEvent.Id);
        var update = Builders<Models.Event>.Update.Set("tickets", foundEvent.Tickets);

        await _eventContext.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        return ticket.Id;
    }
}