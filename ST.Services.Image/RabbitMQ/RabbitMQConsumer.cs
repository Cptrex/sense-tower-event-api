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

    public RabbitMQConsumer(IImageServiceManager imgServiceManager, IConfiguration config)
    {
        try
        {
            foreach (var pair in config.GetChildren())
            {
                Console.WriteLine($"{pair.Path} - {pair.Value}");
            }
            _imageServiceManager = imgServiceManager;

            var IsRunningInContainer = bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), out var inDocker) && inDocker;
            var address = IsRunningInContainer ? "rabbitmq" : "localhost";

            var port = Environment.GetEnvironmentVariable("ServiceEndpoints");
            //var address = config["ServiceEndpoints:EventServiceURL"];
            //var port = Convert.ToInt32(config["ServiceEndpoints:EventServicePort"]);
            var username = "guest";
            var password = "guest";

            var factory = new ConnectionFactory
            {
                HostName = address,
                Port = Convert.ToInt32(port),
                UserName = username,
                Password = password
                //Uri = new Uri($"amqp://{username}:{password}@{address}:{port}/"),
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "image-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine("[IMAGE SERVICE] listening image-queue started");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[IMAGE SERVICE] listening image-queue error: {ex}");
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