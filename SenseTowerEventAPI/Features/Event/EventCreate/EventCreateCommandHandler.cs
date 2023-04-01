using AutoMapper;
using JetBrains.Annotations;
using MediatR;
using SenseTowerEventAPI.Interfaces;
#pragma warning disable CS0618

namespace SenseTowerEventAPI.Features.Event.EventCreate;

[UsedImplicitly]
public class EventCreateCommandHandler : IRequestHandler<EventCreateCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IMongoDBCommunicator _mongoDb;

    public EventCreateCommandHandler(IMapper mapper, IMongoDBCommunicator mongoDb)
    {
        _mapper = mapper;
        _mongoDb = mongoDb;
    }

    public async Task<Guid> Handle(EventCreateCommand request, CancellationToken cancellationToken)
    {
        var newEvent = _mapper.Map<Models.Event>(request);

        await _mongoDb.DbCollection.InsertOneAsync(newEvent, cancellationToken);

        return newEvent.Id;
    }
}