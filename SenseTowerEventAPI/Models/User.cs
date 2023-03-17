using JetBrains.Annotations;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Models;

[UsedImplicitly]
public class User : IUser, IEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public List<ITicket> Tickets { get; set; }

    public User(Guid id, string username, List<ITicket> tickets)
    {
        Id = id;
        Username = username;
        Tickets = tickets;
    }
}
