using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SenseTowerEventAPI.Models.Context;

namespace SenseTowerEventAPI.Features.Event.EventGetList;

[UsedImplicitly]
public class EventGetListQueryHandler : IRequestHandler<EventGetListQuery, List<Models.Event>>
{
    private readonly IMongoCollection<Models.Event> _eventContext;

    public EventGetListQueryHandler(IOptions<EventContext> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);

        _eventContext = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Models.Event>(options.Value.CollectionName);
    }

    public async Task<List<Models.Event>> Handle(EventGetListQuery request, CancellationToken cancellationToken)
    {
        return await _eventContext.Find(_ => true).ToListAsync(cancellationToken: cancellationToken);
    }
}