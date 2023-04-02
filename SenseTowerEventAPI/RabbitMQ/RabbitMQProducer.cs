using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SenseTowerEventAPI.Interfaces;

namespace SenseTowerEventAPI.RabbitMQ;

public class RabbitMQProducer : IRabbitMQProducer
{
    private readonly IModel _channel;

    public RabbitMQProducer(IRabbitMQConfigure rabbitMqConfigure)
    {
        _channel = rabbitMqConfigure.GetRabbitMQChannel();
    }

    public void SendDeleteEventQueueMessage<T>(T message)
    {
        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(exchange: "", routingKey: "event-delete-queue", body: body);

        Console.WriteLine($"{DateTimeOffset.Now} | [EVENT SERVICE] event deleted {message}");
    }
}