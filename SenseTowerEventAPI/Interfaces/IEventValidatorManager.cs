namespace SenseTowerEventAPI.Interfaces;

public interface IEventValidatorManager
{
    public bool IsImageIdExist(Guid imageId);

    public bool IsSpaceIdExist(Guid spaceId);
}