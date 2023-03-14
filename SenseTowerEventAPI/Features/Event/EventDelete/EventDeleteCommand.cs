using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventDelete
{
    public class EventDeleteCommand : IRequest<Guid>, IEntity
    {
        public Guid ID { get; set; }
    }
}