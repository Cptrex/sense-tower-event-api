using JetBrains.Annotations;

namespace SenseTowerEventAPI.Interfaces;

[UsedImplicitly]
public interface IEntity
{
    public Guid Id { get; }
}