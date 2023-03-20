using JetBrains.Annotations;

namespace SenseTowerEventAPI.Interfaces
{
    [UsedImplicitly]
    public interface IEventContext
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
