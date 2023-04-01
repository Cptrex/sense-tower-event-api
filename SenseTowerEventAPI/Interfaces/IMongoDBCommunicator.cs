using MongoDB.Driver;

namespace SenseTowerEventAPI.Interfaces;

public interface IMongoDBCommunicator
{
    IMongoCollection<Models.Event> DbCollection { get; set; }
}