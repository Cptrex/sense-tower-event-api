using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ST.Services.Image.Interfaces;
using ST.Services.Image.Models;

#pragma warning disable CS8618

namespace ST.Services.Image.RabbitMQ;

public class RabbitMQConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IImageServiceManager _imageServiceManager;

    public RabbitMQConsumer(IRabbitMQConfigure rabbitMqConfigure, IImageServiceManager imageServiceManager)
    {
        try
        {
            _imageServiceManager = imageServiceManager;
            _connection = rabbitMqConfigure.GetRabbitMQConnection();
            _channel = rabbitMqConfigure.GetRabbitMQChannel();

            _channel.QueueDeclare(queue: "image-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("[EVENT SERVICE] listening image-queue started");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EVENT SERVICE] listening image-queue error: {ex}");
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
                Console.WriteLine($"{DateTimeOffset.Now} | [IMAGE SERVICE] received cmd: {eventOperation.Type} objectId: {eventOperation.DeletedId}");
                if (eventOperation.Type != EventOperationType.ImageDeleteEvent) return;
                var operationResult = _imageServiceManager.DeleteImageById(eventOperation.DeletedId, stoppingToken).Result;

                Console.WriteLine(operationResult ?
                    $"{DateTimeOffset.Now} | [IMAGE SERVICE] operation result success" : $"{DateTimeOffset.Now} | [IMAGE SERVICE] operation result error");
            };

            _channel.BasicConsume(queue: "image-queue", autoAck: true, consumer: consumer);
        }
        catch
        {
            // ignored
        }

        return Task.CompletedTask;
    }
}