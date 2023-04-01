using JetBrains.Annotations;
using MediatR;
using MongoDB.Driver;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models;

namespace SenseTowerEventAPI.Features.Event.EventDelete;

[UsedImplicitly]
public class EventDeleteCommandHandler : IRequestHandler<EventDeleteCommand, Guid>
{
    private readonly  IRabbitMQProducer _rabbitMQProducer;
    private readonly IMongoDBCommunicator _mongoDb;

    public EventDeleteCommandHandler(IRabbitMQProducer rabbitMqProducer, IMongoDBCommunicator mongoDb)
    {
        _rabbitMQProducer = rabbitMqProducer;
        _mongoDb = mongoDb;
    }

    public async Task<Guid> Handle(EventDeleteCommand request, CancellationToken cancellationToken)
    {
        var deletedEvent = await _mongoDb.DbCollection.FindOneAndDeleteAsync(e=> e.Id == request.Id, cancellationToken: cancellationToken);

        var eventOperationModel = new EventOperationModel
        {
            Type = EventOperationType.EventDeleteEvent, 
            DeletedId = deletedEvent.Id
        };

        _rabbitMQProducer.SendDeleteEventQueueMessage(eventOperationModel);
        return deletedEvent.Id;
    }
}