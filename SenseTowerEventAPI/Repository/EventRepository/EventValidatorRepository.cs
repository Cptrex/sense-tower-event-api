using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Repository.EventRepository;

public class EventValidatorRepository : IEventValidatorRepository
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