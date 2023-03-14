using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventUpdate
{
    public class EventUpdateCommand : IRequest<Guid>, IEvent, IEntity
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public Guid ImageID { get; set; }
        public Guid SpaceID { get; set; }
    }
}