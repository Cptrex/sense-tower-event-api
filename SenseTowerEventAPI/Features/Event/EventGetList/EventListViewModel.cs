using JetBrains.Annotations;

namespace SenseTowerEventAPI.Features.Event.EventGetList;

[UsedImplicitly]
public class EventListViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public Guid ImageId { get; set; }
    public Guid SpaceId { get; set; }
    public List<Models.Ticket> Tickets { get; set; }

    public EventListViewModel(Guid id, string title, DateTime startDate, DateTime endDate,
        string description, Guid imageId, Guid spaceId, List<Models.Ticket> tickets)
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
}