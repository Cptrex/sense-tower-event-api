using RabbitMQ.Client;

namespace ST.Services.Image.Interfaces;

public interface IRabbitMQConfigure
{
    public IModel GetRabbitMQChannel();
    public IConnection GetRabbitMQConnection();
}