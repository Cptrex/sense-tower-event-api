using System.Diagnostics.CodeAnalysis;
using SenseTowerEventAPI.Features.Event.EventUpdate;
#pragma warning disable CS8618

namespace SenseTowerEventAPI.Models;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
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
}