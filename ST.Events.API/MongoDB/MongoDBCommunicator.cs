using MongoDB.Driver;
using ST.Events.API.Interfaces;
using ST.Events.API.Models;

namespace ST.Events.API.MongoDB;

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