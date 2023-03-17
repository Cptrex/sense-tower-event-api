using JetBrains.Annotations;

namespace SenseTowerEventAPI.Interfaces;

[UsedImplicitly]
public interface ITicket
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid Owner { get; set; }
    public string? Place { get; set; }
}