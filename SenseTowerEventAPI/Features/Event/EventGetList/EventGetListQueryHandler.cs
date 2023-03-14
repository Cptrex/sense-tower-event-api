using MediatR;
using SenseTowerEventAPI.Features.Event.EventRead;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventGetList
{
    public class EventGetListQueryHandler : IRequestHandler<EventGetListQuery, List<IEvent>>
    {
        private readonly IEventSingleton _eventInstance;

        public EventGetListQueryHandler(IEventSingleton eventSingleton)
        {
            _eventInstance = eventSingleton;
        }

        public async Task<List<IEvent>> Handle(EventGetListQuery request, CancellationToken cancellationToken)
        {
            return _eventInstance.Events;
        }
    }
}