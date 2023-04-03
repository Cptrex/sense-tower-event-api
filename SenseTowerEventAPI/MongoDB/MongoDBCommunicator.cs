using MongoDB.Driver;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models;

namespace SenseTowerEventAPI.MongoDB;

public class MongoDBCommunicator : IMongoDBCommunicator
{
    public IMongoCollection<Event> DbCollection { get; set; }

    public MongoDBCommunicator()
    {
        var database = Environment.GetEnvironmentVariable("EventsDatabaseSettings__DatabaseName");
        var collection = Environment.GetEnvironmentVariable("EventsDatabaseSettings__CollectionName");
        var connectionString = Environment.GetEnvironmentVariable("EventsDatabaseSettings__ConnectionString");
        var mongoClient = new MongoClient(connectionString);
       
        DbCollection = mongoClient.GetDatabase(database)
            .GetCollection<Event>(collection);
    }
}