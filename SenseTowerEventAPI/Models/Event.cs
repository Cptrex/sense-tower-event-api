using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Models;

public class Event : IEntity, IEvent
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; }

    [BsonElement("startDate")]
    public DateTime StartDate { get; set; }

    [BsonElement("endDate")]
    public DateTime EndDate { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("imageId")]
    public Guid ImageId { get; set; }

    [BsonElement("spaceId")]
    public Guid SpaceId { get; set; }

    [BsonElement("tickets")]
    public List<Ticket> Tickets { get; set; }

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
}