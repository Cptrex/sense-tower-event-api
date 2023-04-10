using ST.Events.API.Models;

namespace ST.Events.API.Interfaces;

public interface IEventSingleton
{
    public List<User> Users { get; set; }
}