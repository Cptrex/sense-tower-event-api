namespace SenseTowerEventAPI.Interfaces;

public interface IRabbitMQProducer
{
    public void SendDeleteEventQueueMessage<T>(T message);
}