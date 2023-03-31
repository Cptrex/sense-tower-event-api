using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.Extensions.Services;

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly IConfiguration _config;

    public RabbitMQProducer(IConfiguration config)
    {
        _config = config;
    }

    public void SendDeleteEventQueueMessage<T>(T message)
    {
        var factory = new ConnectionFactory
        {

            HostName = _config["ServiceEndpoints:EventServiceURL"],
            Port = Convert.ToInt32(_config["ServiceEndpoints:EventServicePort"]),
            UserName = "guest",
            Password = "guest",
            VirtualHost = "/",
            RequestedHeartbeat = new TimeSpan(60),
            Ssl = { ServerName = _config["ServiceEndpoints:EventServiceURL"], Enabled = false }
        };
        var connection = factory.CreateConnection();
        using
            var channel = connection.CreateModel();
        channel.QueueDeclare("event-delete-queue", exclusive: false);
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: "event-delete-queue", body: body);

        //_logger.LogInformation($"{DateTimeOffset.Now} | [EVENT SERVICE] event deleted {message}");
    }
}