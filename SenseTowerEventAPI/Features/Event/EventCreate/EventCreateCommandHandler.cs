using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SenseTowerEventAPI.Models.Context;

namespace SenseTowerEventAPI.Features.Event.EventCreate;

[UsedImplicitly]
public class EventCreateCommandHandler : IRequestHandler<EventCreateCommand, Guid>
{
    private readonly IMongoCollection<Models.Event> _eventContext;

    public EventCreateCommandHandler(IOptions<EventContext> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        _eventContext = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Models.Event>(options.Value.CollectionName);
    }

    public async Task<Guid> Handle(EventCreateCommand request, CancellationToken cancellationToken)
    {
        var newEvent = new Models.Event(Guid.NewGuid(), request.Title, request.StartDate, request.EndDate, request.Description, request.ImageId, request.SpaceId, request.Tickets);

        await _eventContext.InsertOneAsync(newEvent, cancellationToken: cancellationToken);

        return newEvent.Id;
    }
}