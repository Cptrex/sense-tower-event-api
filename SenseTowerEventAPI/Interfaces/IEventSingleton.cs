using SenseTowerEventAPI.Models;

namespace SenseTowerEventAPI.Interfaces;

public interface IEventSingleton
{
    public List<User> Users { get; set; }
}