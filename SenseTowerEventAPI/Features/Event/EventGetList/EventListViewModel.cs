using JetBrains.Annotations;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventGetList;

[UsedImplicitly]
public class EventListViewModel : IEvent, IEntity
{
    public Guid ID { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public Guid ImageID { get; set; }
    public Guid SpaceID { get; set; }
    public List<Models.Ticket> Tickets { get; set; }

    public EventListViewModel(Guid id, string title, DateTime startDate, DateTime endDate,
        string description, Guid imageId, Guid spaceId, List<Models.Ticket> tickets)
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
}