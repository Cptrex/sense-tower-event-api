namespace SenseTowerEventAPI.Interfaces;
public interface IEventValidatorManager
{
    public bool IsImageIdExist(IEventSingleton eventInstance, Guid imageId);

    public bool IsSpaceIdExist(IEventSingleton eventInstance, Guid spaceId);
}