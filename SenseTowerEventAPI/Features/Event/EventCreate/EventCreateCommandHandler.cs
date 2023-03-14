using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventCreate
{
    public class EventCreateCommandHandler : IRequestHandler<EventCreateCommand, Guid>
    {
        private readonly IEventSingleton _eventInstance;

        public EventCreateCommandHandler(IEventSingleton eventSingleton)
        {
            _eventInstance = eventSingleton;
        }

        public async Task<Guid> Handle(EventCreateCommand request, CancellationToken cancellationToken)
        {
            Infastructure.Models.Event newEvent = new Infastructure.Models.Event(Guid.NewGuid(), request.Title, request.StartDate, request.EndDate, request.Description, request.ImageID, request.SpaceID);

            _eventInstance.Events.Add(newEvent);

            return newEvent.ID;
        }
    }
}