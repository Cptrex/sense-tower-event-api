using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SenseTowerEventAPI.Features.Event.EventCreate;
using SenseTowerEventAPI.Features.Event.EventUpdate;
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

    [SuppressMessage("ReSharper", "EmptyConstructor")]
    public  Event() {}

    public void UpdateEvent(EventUpdateCommand request)
    {
        Title = request.Title;
        StartDate = request.StartDate;
        EndDate = request.EndDate;
        Description = request.Description;
        ImageId = request.ImageId;
        SpaceId = request.SpaceId;
        Tickets = request.Tickets;
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