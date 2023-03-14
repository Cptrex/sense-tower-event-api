using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event
{
    public class EventSingleton : IEventSingleton
    {
        public List<IEvent> Events { get; set; } = new List<IEvent>();
        public List<Guid> Spaces { get; set; } = new List<Guid>()         
        {
          new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")
        };

        public List<Guid> Images { get; set; } = new List<Guid>()
        {
          new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")
        };
    }
}