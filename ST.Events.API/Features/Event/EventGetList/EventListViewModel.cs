using JetBrains.Annotations;
// ReSharper disable UnusedMember.Global
#pragma warning disable CS8618

namespace ST.Events.API.Features.Event.EventGetList;

[UsedImplicitly]
public class EventListViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public string Description { get; set; }
    public Guid ImageId { get; set; }
    public Guid SpaceId { get; set; }
    public List<Models.Ticket> Tickets { get; set; }
}