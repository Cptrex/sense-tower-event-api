namespace SenseTowerEventAPI.Interfaces;

public interface IEventSingleton
{
    public List<Guid> Spaces { get; set; }
    public List<Guid> Images { get; set; }
    public List<IUser> Users { get; set; }
}