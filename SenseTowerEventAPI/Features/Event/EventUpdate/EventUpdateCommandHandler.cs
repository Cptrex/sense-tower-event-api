using JetBrains.Annotations;
using MediatR;
using MongoDB.Driver;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Features.Event.EventUpdate;

[UsedImplicitly]
public class EventUpdateCommandHandler : IRequestHandler<EventUpdateCommand, Guid>
{
    private readonly IMongoDBCommunicator _mongoDb;

    public EventUpdateCommandHandler(IMongoDBCommunicator mongoDb)
    {
        _mongoDb = mongoDb;
    }

    public async Task<Guid> Handle(EventUpdateCommand request, CancellationToken cancellationToken)
    {
        var selectedEvent = (await _mongoDb.DbCollection.Find(e => e.Id == request.Id).ToListAsync(cancellationToken)).First();

        selectedEvent.UpdateEvent(request);

        await _mongoDb.DbCollection.ReplaceOneAsync(e=>e.Id == selectedEvent.Id, selectedEvent, cancellationToken: cancellationToken);

        return request.Id;
    }
}