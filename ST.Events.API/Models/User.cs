using JetBrains.Annotations;

namespace ST.Events.API.Models;

[UsedImplicitly]
public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public List<Ticket> Tickets { get; set; }

    public User(Guid id, string username, List<Ticket> tickets)
    {
        Id = id;
        Username = username;
        Tickets = tickets;
    }
}