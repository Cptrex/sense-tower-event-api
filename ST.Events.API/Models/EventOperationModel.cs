using ST.Events.API.Features.Event;

namespace ST.Events.API.Models;

public class EventOperationModel
{
    public EventOperationType Type { get; set; }
    public Guid DeletedId { get; set; }
}