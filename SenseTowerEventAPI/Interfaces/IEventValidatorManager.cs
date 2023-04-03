namespace SenseTowerEventAPI.Interfaces;

public interface IEventValidatorManager
{
    public Task<bool> IsImageIdExist(Guid imageId);

    public Task<bool> IsSpaceIdExist(Guid spaceId);
}