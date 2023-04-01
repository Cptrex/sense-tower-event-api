using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SC.Internship.Common.Exceptions;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.MongoDB.Context;

namespace SenseTowerEventAPI.Features.Ticket.AddTicket;

[UsedImplicitly]
public class AddTicketHandler :IRequestHandler<AddTicketCommand, Guid>
{
    private readonly IMongoCollection<Models.Event> _eventContext;
    private  readonly ITicketManager _ticketRepository;

    public AddTicketHandler(IOptions<EventContext> options, ITicketManager ticketRepository)
    {
        _ticketRepository = ticketRepository;
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _eventContext = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Models.Event>(options.Value.CollectionName);
    }

    public async Task<Guid> Handle(AddTicketCommand request, CancellationToken cancellationToken)
    {
        var foundEvent = ( await _eventContext.Find(e => e.Id == request.Id).ToListAsync(cancellationToken: cancellationToken)).FirstOrDefault();
        
        if (foundEvent == null) throw new ScException("Ошибка добавления билета. Мероприятие не найдено");

        var ticketLastNumber = await _ticketRepository.GetLastTicketNumberInEvent(foundEvent.Id);
        
        var ticket = new Models.Ticket(Guid.NewGuid(),foundEvent.Id, request.Owner, ticketLastNumber + 1, request.Price);
        
        foundEvent.Tickets.Add(ticket);

        var filter = Builders<Models.Event>.Filter.Eq("id", foundEvent.Id);
        var update = Builders<Models.Event>.Update.Set("tickets", foundEvent.Tickets);

        await _eventContext.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        return ticket.Id;
    }
}