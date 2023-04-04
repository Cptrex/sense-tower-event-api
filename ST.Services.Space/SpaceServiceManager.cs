using Newtonsoft.Json;
using ST.Services.Space.Interfaces;
using System.Text;
using ST.Services.Space.Models;
using RabbitMQ.Client;

namespace ST.Services.Space;

public class SpaceServiceManager : ISpaceServiceManager
{
    private readonly ISpaceSingleton _spaceInstance;
    private readonly IModel _channel;

    public SpaceServiceManager(ISpaceSingleton spaceInstance, IRabbitMQConfigure rabbitMqConfigure)
    {
        _spaceInstance = spaceInstance;
        _channel = rabbitMqConfigure.GetRabbitMQChannel();
    }

    public bool DeleteSpaceId(Guid spaceId, CancellationToken cancellationToken)
    {
        var result = _spaceInstance.Spaces.RemoveAll(i => i == spaceId);
        if (result <= 0) return false;

        RemoveEventByUsedSpace(spaceId, cancellationToken);
        
        return true;
    }

    public void RemoveEventByUsedSpace(Guid spaceId, CancellationToken cancellationToken)
    {
        var operationModel = new EventOperationModel
        {
            DeletedId = spaceId,
            Type = EventOperationType.SpaceDeleteEvent
        };

        var json = JsonConvert.SerializeObject(operationModel);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(exchange: "", routingKey: "event-queue", body: body);

        Console.WriteLine($"{DateTimeOffset.Now} | [SPACE SERVICE] send request event-queue {json}");
    }
}