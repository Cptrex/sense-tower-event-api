using JetBrains.Annotations;
using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventGetList;

[UsedImplicitly]
public class EventGetListQueryHandler : IRequestHandler<EventGetListQuery, List<IEvent>>
{
    private readonly IEventSingleton _eventInstance;

    public EventGetListQueryHandler(IEventSingleton eventSingleton)
    {
        _eventInstance = eventSingleton;
    }

    public async Task<List<IEvent>> Handle(EventGetListQuery request, CancellationToken cancellationToken)
    {
        return await  Task.FromResult(_eventInstance.Events);
    }
}