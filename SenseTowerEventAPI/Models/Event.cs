using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Models;

public class Event : IEntity, IEvent
{
    public Guid ID { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public Guid ImageID { get; set; }
    public Guid SpaceID { get; set; }
    public List<Ticket> Tickets { get; set; }

    public Event(Guid id, string title, DateTime startDate, DateTime endDate, string description, Guid imageId, Guid spaceId, List<Ticket> tickets)
    {
        ID = id;
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        ImageID = imageId;
        SpaceID = spaceId;
        Tickets = tickets;
    }

    public void UpdateEvent(DateTime startDate, DateTime endDate, string title, string description, Guid imageId, Guid spaceId)
    {
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        ImageID = imageId;
        SpaceID = spaceId;
    }
}