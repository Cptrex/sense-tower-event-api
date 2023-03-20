using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SenseTowerEventAPI.Models.Context;

namespace SenseTowerEventAPI.Features.Event.EventDelete;

[UsedImplicitly]
public class EventDeleteCommandHandler : IRequestHandler<EventDeleteCommand, Guid>
{
    private readonly IMongoCollection<Models.Event> _eventContext;

    public EventDeleteCommandHandler(IOptions<EventContext> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _eventContext = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Models.Event>(options.Value.CollectionName);
    }

    public async Task<Guid> Handle(EventDeleteCommand request, CancellationToken cancellationToken)
    {
        var deletedEvent = await _eventContext.FindOneAndDeleteAsync(e=> e.Id == request.Id, cancellationToken: cancellationToken);

        return deletedEvent.Id;
    }
}