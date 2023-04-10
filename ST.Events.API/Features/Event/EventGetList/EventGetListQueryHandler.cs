using JetBrains.Annotations;
using MediatR;
using MongoDB.Driver;
using ST.Events.API.Interfaces;

namespace ST.Events.API.Features.Event.EventGetList;

[UsedImplicitly]
public class EventGetListQueryHandler : IRequestHandler<EventGetListQuery, List<Models.Event>>
{
    private readonly IMongoDBCommunicator _mongoDb;

    public EventGetListQueryHandler(IMongoDBCommunicator mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task<List<Models.Event>> Handle(EventGetListQuery request, CancellationToken cancellationToken)
    {
        return await _mongoDb.DbCollection.Find(_ => true).ToListAsync(cancellationToken);
    }
}