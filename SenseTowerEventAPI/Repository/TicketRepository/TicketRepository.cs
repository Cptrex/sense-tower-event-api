using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models.Context;
using System.Text;
using Newtonsoft.Json;
using SC.Internship.Common.ScResult;

namespace SenseTowerEventAPI.Repository.TicketRepository;

public class TicketRepository : ITicketRepository
{
    private readonly IMongoCollection<Models.Event> _eventContext;
    private readonly HttpClient _httpClient;

    public TicketRepository(HttpClient httpClient, IOptions<EventContext> options)
    {
        _httpClient = httpClient;
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

    public async Task<Guid> CreateTicketPayment()
    {
        HttpContent content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        var result = await _httpClient.PostAsync(new Uri("/payment"), content);

        var response = JsonConvert.DeserializeObject<ScResult<Guid>>(await result.Content.ReadAsStringAsync());

        if (response is null) return Guid.Empty;

        var paymentTransactionId = response.Result;

        return paymentTransactionId;
    }

    public async Task CancelTicketPayment(Guid createdTransactionId)
    {
        var payload = JsonConvert.SerializeObject(createdTransactionId);
        HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

        await _httpClient.PatchAsync(new Uri("/payment"), content);
    }

    public async Task ConfirmTicketPayment(Guid createdTransactionId)
    {
        var payload = JsonConvert.SerializeObject(createdTransactionId);
        HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");

        await _httpClient.PutAsync(new Uri("/payment"), content);
    }
}