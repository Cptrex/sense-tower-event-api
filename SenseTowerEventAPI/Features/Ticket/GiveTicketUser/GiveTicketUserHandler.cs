using JetBrains.Annotations;
using MediatR;
using MongoDB.Driver;
using SC.Internship.Common.Exceptions;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Ticket.GiveTicketUser;

[UsedImplicitly]
public class GiveTicketUserHandler : IRequestHandler<GiveTicketUserCommand, Guid>
{
    private readonly IMongoCollection<Models.Event> _eventContext;
    private readonly IEventSingleton _evenInstance;
    private readonly ITicketManager _ticketRepository;

    public GiveTicketUserHandler(IEventSingleton evenInstance, ITicketManager ticketRepository, IMongoDBCommunicator mongoDbCommunicator)
    {
        _evenInstance = evenInstance;
        _ticketRepository = ticketRepository;
        _eventContext = mongoDbCommunicator.DbCollection;
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