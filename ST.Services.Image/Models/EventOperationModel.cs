namespace ST.Services.Image.Models;

public class EventOperationModel
{
    public EventOperationType Type { get; set; }
    public Guid DeletedId { get; set; }
}