namespace ST.Events.API.Interfaces;

public interface IRabbitMQProducer
{
    public void SendDeleteEventQueueMessage<T>(T message);
}