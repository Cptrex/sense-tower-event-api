using RabbitMQ.Client;
using ST.Services.Image.Interfaces;

namespace ST.Services.Image.RabbitMQ;

public class RabbitMQConfigure : IRabbitMQConfigure
{
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public RabbitMQConfigure(IConfiguration config)
    {
        var factory = new ConnectionFactory
        {
            HostName = config["ServiceEndpoints:RabbitMQ:Hostname"],
            Port = Convert.ToInt32(config["ServiceEndpoints:RabbitMQ:Port"]),
            UserName = config["ServiceEndpoints:RabbitMQ:User"],
            Password = config["ServiceEndpoints:RabbitMQ:Password"],
            VirtualHost = "/",
            RequestedHeartbeat = new TimeSpan(60),
            Ssl = { ServerName = config["ServiceEndpoints:RabbitMQ:Hostname"], Enabled = false }
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