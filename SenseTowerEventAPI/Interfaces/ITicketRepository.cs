using JetBrains.Annotations;

namespace SenseTowerEventAPI.Interfaces;
[UsedImplicitly]
public interface ITicketRepository
{
    public Task<List<Models.Ticket>> GetAllEventTickets(Guid eventId);
    public Task<int> GetLastTicketNumberInEvent(Guid eventId);
}