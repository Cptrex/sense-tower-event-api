namespace SenseTowerEventAPI.Interfaces;
public interface IEventValidatorRepository
{
    public bool IsImageIdExist(IEventSingleton eventInstance, Guid imageId);

    public bool IsSpaceIdExist(IEventSingleton eventInstance, Guid spaceId);
}