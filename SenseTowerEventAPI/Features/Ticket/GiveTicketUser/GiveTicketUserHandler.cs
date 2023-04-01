using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SC.Internship.Common.Exceptions;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.MongoDB.Context;

namespace SenseTowerEventAPI.Features.Ticket.GiveTicketUser;

[UsedImplicitly]
public class GiveTicketUserHandler : IRequestHandler<GiveTicketUserCommand, Guid>
{
    private readonly IMongoCollection<Models.Event> _eventContext;
    private readonly IEventSingleton _evenInstance;
    private readonly ITicketManager _ticketRepository;

    public GiveTicketUserHandler(IEventSingleton evenInstance, IOptions<EventContext> options, ITicketManager ticketRepository)
    {
        _evenInstance = evenInstance;
        _ticketRepository = ticketRepository;
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _eventContext = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Models.Event>(options.Value.CollectionName);
    }

    public async Task<Guid> Handle(GiveTicketUserCommand request, CancellationToken cancellationToken)
    {
        var foundUser = _evenInstance.Users.FirstOrDefault(u => u.Id == request.Owner);
        
        if (foundUser == null) throw new ScException("Не удалось выдать билет. Пользователь не найден");

        var eventTickers = await _ticketRepository.GetAllEventTickets(request.EventId);
        var createPaymentTransactionId  = await _ticketRepository.CreateTicketPayment();

        if (createPaymentTransactionId == Guid.Empty) await _ticketRepository.CancelTicketPayment(createPaymentTransactionId);

        eventTickers.RemoveAll(t => t.Id == request.Id);

        var update = Builders<Models.Event>.Update.Set("tickets", eventTickers);

        await _eventContext.UpdateOneAsync(e=> e.Id == request.EventId, update, cancellationToken: cancellationToken);

        await _ticketRepository.ConfirmTicketPayment(createPaymentTransactionId);

        return request.Owner;
    }
}