using JetBrains.Annotations;
using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventCreate;

[UsedImplicitly]
public class EventCreateCommandHandler : IRequestHandler<EventCreateCommand, Guid>
{
    private readonly IEventSingleton _eventInstance;

    public EventCreateCommandHandler(IEventSingleton eventSingleton)
    {
        _eventInstance = eventSingleton;
    }

    public async Task<Guid> Handle(EventCreateCommand request, CancellationToken cancellationToken)
    {
        var newEvent = new Models.Event(Guid.NewGuid(), request.Title, request.StartDate, request.EndDate, request.Description, request.ImageID, request.SpaceID, request.Tickets);

        _eventInstance.Events.Add(newEvent);

        return await Task.FromResult(newEvent.ID);
    }
}