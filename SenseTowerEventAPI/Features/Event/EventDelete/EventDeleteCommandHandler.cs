using JetBrains.Annotations;
using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventDelete;

[UsedImplicitly]
public class EventDeleteCommandHandler : IRequestHandler<EventDeleteCommand, Guid>
{
    private readonly IEventSingleton _eventInstance;

    public EventDeleteCommandHandler(IEventSingleton eventSingleton)
    {
        _eventInstance = eventSingleton;
    }
    public async Task<Guid> Handle(EventDeleteCommand request, CancellationToken cancellationToken)
    {
        _eventInstance.Events.RemoveAll(e=> e.Id == request.Id);

        return await Task.FromResult(request.Id);
    }
}