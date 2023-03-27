using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ST.Services.Space.Interfaces;
using ST.Services.Space.Models;
using ST.Services.Space.Utils;

namespace ST.Services.Space.Extensions.Services;

public class RabbitMQConsumerService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ISpaceRepository _imageRepository;

    public RabbitMQConsumerService(ISpaceRepository imgRepository, IConfiguration config)
    {
        _imageRepository = imgRepository;

        var factory = new ConnectionFactory
        {

            HostName = config["ServiceEndpoints:EventServiceURL"],
            Port = Convert.ToInt32(config["ServiceEndpoints:EventServicePort"]),
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            RequestedHeartbeat = new TimeSpan(60),
            Ssl = { ServerName = config["ServiceEndpoints:EventServiceURL"], Enabled = false }
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "space-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        Console.WriteLine("[SPACE SERVICE] listening space-queue started");
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
            Console.WriteLine($"{DateTime.Now} | [SPACE SERVICE] received cmd: {eventOperation.Type} objectId: {eventOperation.DeletedId}");
            if (eventOperation.Type != EventOperationType.SpaceDeleteEvent) return;
            var operationResult =  _imageRepository.DeleteSpaceId(eventOperation.DeletedId, stoppingToken).Result;

            Console.WriteLine(operationResult ? 
                $"{DateTime.Now} | [SPACE SERVICE] operation result success" : $"{DateTime.Now} | [SPACE SERVICE] operation result error");
        };

        _channel.BasicConsume(queue: "space-queue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}