using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Models.Context
{
    public class EventContext : IEventContext
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string CollectionName { get; set; } = string.Empty;
    }
}