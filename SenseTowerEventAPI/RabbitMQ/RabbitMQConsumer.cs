using System.Text;
using JetBrains.Annotations;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SC.Internship.Common.Exceptions;
using SenseTowerEventAPI.Features.Event;
using SenseTowerEventAPI.Interfaces;
using SenseTowerEventAPI.Models;

#pragma warning disable CS8618

namespace SenseTowerEventAPI.RabbitMQ;

[UsedImplicitly]
public class RabbitMQConsumer : BackgroundService
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly IMongoDBCommunicator _mongoDb;

    public RabbitMQConsumer(IRabbitMQConfigure rabbitMqConfigure, IMongoDBCommunicator mongoDb)
    {
        try
        {
            _mongoDb = mongoDb;
            _connection = rabbitMqConfigure.GetRabbitMQConnection();
            _channel = rabbitMqConfigure.GetRabbitMQChannel();

            _channel.QueueDeclare(queue: "event-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("[EVENT SERVICE] listening event-queue started");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EVENT SERVICE] listening event-queue error: {ex}");
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();

                return Task.CompletedTask;
            }

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                EventOperationModel? eventOperation;
                try
                {
                    eventOperation = JsonConvert.DeserializeObject<EventOperationModel>(message);
                }
                catch
                {
                    eventOperation = null;
                }

                if (eventOperation == null) return;
                
                Console.WriteLine($"{DateTimeOffset.Now} | [EVENT SERVICE] received cmd: {eventOperation.Type} objectId: {eventOperation.DeletedId}");
                bool operationResult;

                switch (eventOperation.Type)
                {
                    case EventOperationType.ImageDeleteEvent:
                        var filter = Builders<Event>.Filter.Eq(e=> e.ImageId, eventOperation.DeletedId);
                        var update = Builders<Event>.Update.Set(e => e.ImageId, Guid.Empty);

                        _mongoDb.DbCollection.UpdateMany(filter, update);
                        operationResult = true;
                        break;
                    case EventOperationType.SpaceDeleteEvent:
                        filter = Builders<Event>.Filter.Eq(e=> e.SpaceId, eventOperation.DeletedId);

                        var result = _mongoDb.DbCollection.DeleteMany(filter);
                        Console.WriteLine($"DeletedCount: {result.DeletedCount}");
                        operationResult = true;
                        break;
                    case EventOperationType.EventDeleteEvent:
                    default:
                        throw new ScException("Такого события очереди не существует");
                }

                Console.WriteLine(operationResult ?
                    $"{DateTimeOffset.Now} | [EVENT SERVICE] operation result success" : $"{DateTimeOffset.Now} | [EVENT SERVICE] operation result error");
            };

            _channel.BasicConsume(queue: "event-queue", autoAck: true, consumer: consumer);
        }
        catch
        {
            // ignored
        }

        return Task.CompletedTask;
    }
}