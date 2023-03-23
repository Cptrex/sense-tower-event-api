using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models.Context;

namespace SenseTowerEventAPI.Repository.TicketRepository;

public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<Models.Event> _eventContext;

    public TicketRepository(IOptions<EventContext> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _eventContext = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Models.Event>(options.Value.CollectionName);
    }
    public async Task<List<Models.Ticket>> GetAllEventTickets(Guid eventId)
    {
        var foundEvent = (await _eventContext.Find(e => e.Id == eventId).ToListAsync()).First();
           
        return foundEvent.Tickets;
    }

    public async Task<int> GetLastTicketNumberInEvent(Guid eventId)
    {
        var tickets = await GetAllEventTickets(eventId);
        var lastTickerNumber = tickets.OrderBy(t => t.PlaceNumber).First().PlaceNumber;

        return  lastTickerNumber;
    }
}