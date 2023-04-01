namespace ST.Services.Space.Models;

public class EventOperationModel
{
    public EventOperationType Type { get; set; }
    public Guid DeletedId { get; set; }
}