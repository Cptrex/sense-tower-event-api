using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Models;

public class Event : IEntity, IEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public Guid ImageId { get; set; }
    public Guid SpaceId { get; set; }
    public List<ITicket> Tickets { get; set; }

    public Event(Guid id, string title, DateTime startDate, DateTime endDate, string description, Guid imageId, Guid spaceId, List<ITicket> tickets)
    {
        Id = id;
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        ImageId = imageId;
        SpaceId = spaceId;
        Tickets = tickets;
    }

    public void UpdateEvent(DateTime startDate, DateTime endDate, string title, string description, Guid imageId, Guid spaceId)
    {
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        ImageId = imageId;
        SpaceId = spaceId;
    }
}