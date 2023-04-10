using MongoDB.Driver;

namespace ST.Events.API.Interfaces;

public interface IMongoDBCommunicator
{
    IMongoCollection<Models.Event> DbCollection { get; set; }
}