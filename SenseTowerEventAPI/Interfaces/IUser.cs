using JetBrains.Annotations;

namespace SenseTowerEventAPI.Interfaces
{
    [UsedImplicitly]
    public interface IUser
    {
        public Guid Id { get; set; }
        string Username { get; set; }
        List<ITicket> Tickets { get; set; }
    }
}