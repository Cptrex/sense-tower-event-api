using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SenseTowerEventAPI.Models.Context;

namespace SenseTowerEventAPI.Features.Event.EventUpdate;

[UsedImplicitly]
public class EventUpdateCommandHandler : IRequestHandler<EventUpdateCommand, Guid>
{
    private readonly IMongoCollection<Models.Event> _eventContext;

    public EventUpdateCommandHandler(IOptions<EventContext> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _eventContext = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Models.Event>(options.Value.CollectionName);
    }

    public async Task<Guid> Handle(EventUpdateCommand request, CancellationToken cancellationToken)
    {
        var selectedEvent = (await _eventContext.Find(e => e.Id == request.Id).ToListAsync(cancellationToken: cancellationToken)).First();

        selectedEvent.UpdateEvent(request.StartDate, request.EndDate, request.Title, request.Description, request.ImageId, request.SpaceId, request.Tickets);

        await _eventContext.ReplaceOneAsync(e=>e.Id == selectedEvent.Id, selectedEvent, cancellationToken: cancellationToken);

        return request.Id;
    }
}