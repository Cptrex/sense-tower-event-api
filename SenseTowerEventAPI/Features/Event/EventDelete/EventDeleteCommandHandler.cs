using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models;
using SenseTowerEventAPI.Models.Context;
using SenseTowerEventAPI.Utils;

namespace SenseTowerEventAPI.Features.Event.EventDelete;

[UsedImplicitly]
public class EventDeleteCommandHandler : IRequestHandler<EventDeleteCommand, Guid>
{
    private readonly IMongoCollection<Models.Event> _eventContext;
    private readonly  IRabbitMQProducer _rabbitMQProducer;
    public EventDeleteCommandHandler(IOptions<EventContext> options, IRabbitMQProducer rabbitMqProducer)
    {
        _rabbitMQProducer = rabbitMqProducer;
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _eventContext = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Models.Event>(options.Value.CollectionName);
    }

    public async Task<Guid> Handle(EventDeleteCommand request, CancellationToken cancellationToken)
    {
        var deletedEvent = await _eventContext.FindOneAndDeleteAsync(e=> e.Id == request.Id, cancellationToken: cancellationToken);

        var eventOperationModel = new EventOperationModel
        {
            Type = EventOperationType.EventDeleteEvent, 
            DeletedId = deletedEvent.Id
        };

        _rabbitMQProducer.SendDeleteEventQueueMessage(eventOperationModel);
        return deletedEvent.Id;
    }
}