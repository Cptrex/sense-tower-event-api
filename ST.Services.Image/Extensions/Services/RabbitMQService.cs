using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ST.Services.Image.Interfaces;
using ST.Services.Image.Models;
using ST.Services.Image.Utils;

namespace ST.Services.Image.Extensions.Services;

public class RabbitMQConsumerService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IImageRepository _imageRepository;

    public RabbitMQConsumerService(IImageRepository imgRepository)
    {
        _imageRepository = imgRepository;

        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            RequestedHeartbeat = new TimeSpan(60),
            Ssl = { ServerName = "localhost", Enabled = false }
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "image-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        Console.WriteLine("[IMAGE SERVICE] listening image-queue started");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
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
            Console.WriteLine($"{DateTime.Now} | [IMAGE SERVICE] received cmd: {eventOperation.Type} objectId: {eventOperation.DeletedId}");
            if (eventOperation.Type != EventOperationType.ImageDeleteEvent) return;
            var operationResult =  _imageRepository.DeleteImageById(eventOperation.DeletedId, stoppingToken).Result;

            Console.WriteLine(operationResult ? 
                $"{DateTime.Now} | [IMAGE SERVICE] operation result success" : $"{DateTime.Now} | [IMAGE SERVICE] operation result error");
        };

        _channel.BasicConsume(queue: "image-queue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}