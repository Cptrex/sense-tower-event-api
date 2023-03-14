using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Infastructure.Models
{
    public class Event : IEntity, IEvent
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public Guid ImageID { get; set; }
        public Guid SpaceID { get; set; }

        public Event(Guid id, string title, DateTime startDate, DateTime endDate, string description, Guid imageID, Guid spaceID)
        {
            ID = id;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            ImageID = imageID;
            SpaceID = spaceID;
        }

        public void UpdateEvent(DateTime startDate, DateTime endDate, string title, string description, Guid imageID, Guid spaceID)
        {
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Description = description;
            ImageID = imageID;
            SpaceID = spaceID;
        }
    }
}