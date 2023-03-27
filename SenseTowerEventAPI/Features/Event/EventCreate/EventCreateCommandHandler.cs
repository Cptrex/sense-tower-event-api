using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SenseTowerEventAPI.Models.Context;
#pragma warning disable CS0618

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
        var config = new MapperConfiguration(cfg => cfg.CreateMap<EventCreateCommand, Models.Event>());
        var mapper = new Mapper(config);

        var newEvent = mapper.Map<EventCreateCommand, Models.Event>(request);

        await _eventContext.InsertOneAsync(newEvent, cancellationToken);

        return newEvent.Id;
    }
}