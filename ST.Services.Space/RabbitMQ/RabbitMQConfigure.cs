using RabbitMQ.Client;
using ST.Services.Space.Interfaces;

namespace ST.Services.Space.RabbitMQ;

public class RabbitMQConfigure : IRabbitMQConfigure
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public RabbitMQConfigure()
    {
        var factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("ServiceEndpoints__RabbitMQ__Hostname"),
            Port = Convert.ToInt32(Environment.GetEnvironmentVariable("ServiceEndpoints__RabbitMQ__Port")),
            UserName = Environment.GetEnvironmentVariable("ServiceEndpoints__RabbitMQ__User"),
            Password = Environment.GetEnvironmentVariable("ServiceEndpoints__RabbitMQ__Password"),
            VirtualHost = "/",
            RequestedHeartbeat = new TimeSpan(60),
            Ssl = { ServerName = Environment.GetEnvironmentVariable("ServiceEndpoints__RabbitMQ__Hostname"), Enabled = false }
        }; 
        
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public IModel GetRabbitMQChannel()
    {
        return _channel;
    }

    public IConnection GetRabbitMQConnection()
    {
        return _connection;
    }
}