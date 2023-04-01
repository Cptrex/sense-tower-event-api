using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event;

public class EventValidatorManager : IEventValidatorManager
{
    public bool IsImageIdExist(IEventSingleton eventInstance, Guid imageId)
    {
        return eventInstance.Images.Contains(imageId);
    }

    public bool IsSpaceIdExist(IEventSingleton eventInstance, Guid spaceId)
    {
        return eventInstance.Spaces.Contains(spaceId);
    }
}