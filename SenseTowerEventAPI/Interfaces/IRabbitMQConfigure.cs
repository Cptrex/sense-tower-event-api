using RabbitMQ.Client;

namespace SenseTowerEventAPI.Interfaces;

public interface IRabbitMQConfigure
{
    public IModel GetRabbitMQChannel();
    public IConnection GetRabbitMQConnection();
}