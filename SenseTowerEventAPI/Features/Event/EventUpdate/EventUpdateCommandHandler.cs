﻿using JetBrains.Annotations;
using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventUpdate;

[UsedImplicitly]
public class EventUpdateCommandHandler : IRequestHandler<EventUpdateCommand, Guid>
{
    private readonly IEventSingleton _eventInstance;

    public EventUpdateCommandHandler(IEventSingleton eventSingleton)
    {
        _eventInstance = eventSingleton;
    }

    public async Task<Guid> Handle(EventUpdateCommand request, CancellationToken cancellationToken)
    {
        var selectedEvent = (Models.Event) _eventInstance.Events.FirstOrDefault(e=> e.Id == request.Id)!;

        selectedEvent.UpdateEvent(request.StartDate, request.EndDate, request.Title, request.Description, request.ImageId, request.SpaceId);

        return await Task.FromResult(request.Id);
    }
}