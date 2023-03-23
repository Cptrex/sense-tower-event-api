using SenseTowerEventAPI.Models;

namespace SenseTowerEventAPI.Interfaces;

public interface IEventSingleton
{
    public List<Guid> Spaces { get; set; }
    public List<Guid> Images { get; set; }
    public List<User> Users { get; set; }
}