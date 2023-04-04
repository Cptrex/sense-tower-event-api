using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ST.Services.Space.Interfaces;
using ST.Services.Space.Models;

#pragma warning disable CS8618

namespace ST.Services.Space.RabbitMQ;

public class RabbitMQConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ISpaceServiceManager _spaceServiceManager;

    public RabbitMQConsumer(IRabbitMQConfigure rabbitMqConfigure, ISpaceServiceManager spaceServiceManager)
    {
        try
        {
            _spaceServiceManager = spaceServiceManager;
            _connection = rabbitMqConfigure.GetRabbitMQConnection();
            _channel = rabbitMqConfigure.GetRabbitMQChannel();

            _channel.QueueDeclare(queue: "space-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("[SPACE SERVICE] listening space-queue started");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SPACE SERVICE] listening space-queue error: {ex}");
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
                Console.WriteLine($"{DateTimeOffset.Now} | [SPACE SERVICE] received cmd: {eventOperation.Type} objectId: {eventOperation.DeletedId}");
                if (eventOperation.Type != EventOperationType.SpaceDeleteEvent) return;
                var operationResult = _spaceServiceManager.DeleteSpaceId(eventOperation.DeletedId, stoppingToken);

                Console.WriteLine(operationResult ?
                    $"{DateTimeOffset.Now} | [SPACE SERVICE] operation result success" : $"{DateTimeOffset.Now} | [SPACE SERVICE] operation result error");
            };

            _channel.BasicConsume(queue: "space-queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
        catch
        {
            //ignored
        }

        return Task.CompletedTask;
    }
}