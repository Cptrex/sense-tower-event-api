using JetBrains.Annotations;

namespace SenseTowerEventAPI.Interfaces;

public interface IEventContext
{
    [UsedImplicitly]
    public string ConnectionString { get; set; }
    [UsedImplicitly]
    public string DatabaseName { get; set; }
    [UsedImplicitly]
    public string CollectionName { get; set; }
}