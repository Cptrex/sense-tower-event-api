using JetBrains.Annotations;

namespace SenseTowerEventAPI.Models;

[UsedImplicitly]
public class User
{
    public Guid ID { get; set; }
    public string Username { get; set; }

    public User(Guid id, string username)
    {
        ID = id;
        Username = username;
    }
}
