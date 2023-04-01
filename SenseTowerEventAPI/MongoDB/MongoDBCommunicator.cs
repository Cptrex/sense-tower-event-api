using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models;
using SenseTowerEventAPI.MongoDB.Context;

namespace SenseTowerEventAPI.MongoDB;

public class MongoDBCommunicator : IMongoDBCommunicator
{
    public IMongoCollection<Event> DbCollection { get; set; }

    public MongoDBCommunicator(IOptions<EventContext> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        DbCollection = mongoClient.GetDatabase(options.Value.DatabaseName)
            .GetCollection<Event>(options.Value.CollectionName);
    }
}