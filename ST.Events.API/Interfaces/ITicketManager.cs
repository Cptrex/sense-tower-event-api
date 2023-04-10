using JetBrains.Annotations;

namespace ST.Events.API.Interfaces;
[UsedImplicitly]
public interface ITicketManager
{
    public Task<List<Models.Ticket>> GetAllEventTickets(Guid eventId);
    public Task<int> GetLastTicketNumberInEvent(Guid eventId);
    public Task<Guid> CreateTicketPayment();
    public Task ConfirmTicketPayment(Guid createdTransactionId);
    public Task CancelTicketPayment(Guid createdTransactionId);
}