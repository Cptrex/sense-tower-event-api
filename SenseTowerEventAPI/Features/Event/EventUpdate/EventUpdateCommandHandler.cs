using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventUpdate
{
    public class EventUpdateCommandHandler : IRequestHandler<EventUpdateCommand, Guid>
    {
        private readonly IEventSingleton _eventInstance;

        public EventUpdateCommandHandler(IEventSingleton eventSingleton)
        {
            _eventInstance = eventSingleton;
        }

        public async Task<Guid> Handle(EventUpdateCommand request, CancellationToken cancellationToken)
        {
           Infastructure.Models.Event selectedEvent = (Infastructure.Models.Event) _eventInstance.Events.FirstOrDefault(e=> e.ID == request.ID);

            if(selectedEvent == null) return Guid.Empty;

            selectedEvent.UpdateEvent(request.StartDate, request.EndDate, request.Title, request.Description, request.ImageID, request.SpaceID);

            return request.ID;
        }
    }
}