using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Models;

public class EventSingleton : IEventSingleton
{
    public List<IEvent> Events { get; set; } = new();
    public List<Guid> Spaces { get; set; } = new()
    {
        new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")
    };
    public List<Guid> Images { get; set; } = new()
    {
        new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")
    };
}