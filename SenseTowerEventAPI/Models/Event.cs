using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SenseTowerEventAPI.Features.Event.EventCreate;
#pragma warning disable CS8618

namespace SenseTowerEventAPI.Models;

public class Event
{
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public Guid ImageId { get; set; }
    public Guid SpaceId { get; set; }
    public List<Ticket> Tickets { get; set; }

    public  Event() {}

    public Event(Guid id, string title, DateTime startDate, DateTime endDate, string description, Guid imageId, Guid spaceId, List<Ticket> tickets)
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

    public void UpdateEvent(DateTime startDate, DateTime endDate, string title, string description, Guid imageId, Guid spaceId, List<Ticket> tickets)
    {
        Title = title;
        StartDate = startDate;
        EndDate = endDate;
        Description = description;
        ImageId = imageId;
        SpaceId = spaceId;
        Tickets = tickets;
    }

    public void InitEventCreateCommand(EventCreateCommand cmd)
    {
        Title = cmd.Title;
        StartDate = cmd.StartDate;
        EndDate = cmd.EndDate;
        Description = cmd.Description;
        ImageId = cmd.ImageId;
        SpaceId = cmd.SpaceId;
        Tickets = cmd.Tickets;
    }
}