using JetBrains.Annotations;

namespace SenseTowerEventAPI.Interfaces;

[UsedImplicitly]
public interface ITicket
{
    public  Guid ID { get; set; }
    public string Owner { get; set; }
    public string? Place { get; set; }
}