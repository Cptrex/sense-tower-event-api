using RabbitMQ.Client;

namespace ST.Events.API.Interfaces;

public interface IRabbitMQConfigure
{
    public IModel GetRabbitMQChannel();
    public IConnection GetRabbitMQConnection();
}