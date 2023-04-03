using RabbitMQ.Client;

namespace ST.Services.Space.Interfaces;

public interface IRabbitMQConfigure
{
    public IModel GetRabbitMQChannel();
    public IConnection GetRabbitMQConnection();
}