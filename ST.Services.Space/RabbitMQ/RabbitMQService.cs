using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ST.Services.Space.Interfaces;
using ST.Services.Space.Models;

#pragma warning disable CS8618

namespace ST.Services.Space.RabbitMQ;

public class RabbitMQConsumerService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ISpaceServiceManager _imageRepository;

    public RabbitMQConsumerService(ISpaceServiceManager imgRepository, IConfiguration config)
    {
        try
        {
            var IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;
            var address = IsRunningInContainer ? "rabbitmq" : "localhost";

            _imageRepository = imgRepository;

            //var address = config["ServiceEndpoints:EventServiceURL"];
            var port = Convert.ToInt32(config["ServiceEndpoints:EventServicePort"]);
            var username = "guest";
            var password = "guest";

            var factory = new ConnectionFactory
            {
                Uri = new Uri($"amqp://{username}:{password}@{address}:{port}/"),
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "space-queue", durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            Console.WriteLine("[SPACE SERVICE] listening space-queue started");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SPACE SERVICE] listening space-queue error : {ex}");
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
                var operationResult = _imageRepository.DeleteSpaceId(eventOperation.DeletedId, stoppingToken).Result;

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