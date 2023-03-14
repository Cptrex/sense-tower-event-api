using MediatR;
using SenseTowerEventAPI.Interfaces;
using System.Linq;
namespace SenseTowerEventAPI.Features.Event.EventDelete
{
    public class EventDeleteCommandHandler : IRequestHandler<EventDeleteCommand, Guid>
    {
        private readonly IEventSingleton _eventInstance;

        public EventDeleteCommandHandler(IEventSingleton eventSingleton)
        {
            _eventInstance = eventSingleton;
        }
        public async Task<Guid> Handle(EventDeleteCommand request, CancellationToken cancellationToken)
        {
            _eventInstance.Events.RemoveAll(e=> e.ID == request.ID);

            return request.ID;
        }
    }
}