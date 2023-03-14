using MediatR;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventCreate
{
    public class EventCreateCommand : IRequest<Guid>, IEvent
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddHours(1);
        public string Description { get; set; }
        public Guid ImageID { get; set ; }
        public Guid SpaceID { get; set ; }
    }
}