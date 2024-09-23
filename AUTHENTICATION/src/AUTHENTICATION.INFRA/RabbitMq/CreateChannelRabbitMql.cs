using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Configuration;
using System.Data.Entity;

namespace AUTHENTICATION.INFRA.RabbitMq;

public class CreateChannelRabbitMql : ICreateChannelRabbitMql
{
    private IConnection _connection;
    private IModel _channel;
    private string RABBIT_HOST;
    private string RABBIT_PORT;
    private string RABBIT_USERNAME;
    private string RABBIT_PASSWORD;
    private readonly ILogger<ConsumersRabbit> _logger;
    private readonly IConfiguration _configuration;

    public CreateChannelRabbitMql(ILogger<ConsumersRabbit> logger,
        IConfiguration configuration)
    {
        RABBIT_HOST = configuration.GetSection("RabbitMqSettings")["HOST"] ?? string.Empty;
        RABBIT_PORT = configuration.GetSection("RabbitMqSettings")["PORT"] ?? string.Empty;
        RABBIT_USERNAME = configuration.GetSection("RabbitMqSettings")["USERNAME"] ?? string.Empty;
        RABBIT_PASSWORD = configuration.GetSection("RabbitMqSettings")["PASSWORD"] ?? string.Empty;
        _logger = logger;
        _configuration = configuration;
        CreateConnection();
    }

    private void CreateConnection()
    {
        try
        {
            var rabbitMQConfig = _configuration.GetSection("RabbitMQ");
            if (_channel is not null) return;

            _connection = new ConnectionFactory
            {
                AmqpUriSslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                Endpoint = new AmqpTcpEndpoint(RABBIT_HOST, int.Parse(RABBIT_PORT)),
                UserName = RABBIT_USERNAME,
                Password = RABBIT_PASSWORD
            }.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: EventConstants.CREATE_DOCTOR_EXCHANGE,
                type: ExchangeType.Direct);

            _channel.QueueDeclare(
                queue: EventConstants.CREATE_DOCTOR_QUEUE,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(
                exchange: EventConstants.CREATE_DOCTOR_EXCHANGE,
                queue: EventConstants.CREATE_DOCTOR_QUEUE,
            routingKey: string.Empty);

            _channel.ExchangeDeclare(
               exchange: EventConstants.CREATE_PATIENT_EXCHANGE,
               type: ExchangeType.Direct);

            _channel.QueueDeclare(
                queue: EventConstants.CREATE_PATIENT_QUEUE,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind(
                exchange: EventConstants.CREATE_PATIENT_EXCHANGE,
                queue: EventConstants.CREATE_PATIENT_QUEUE,
            routingKey: string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while creating RabbitMq Connection.");
            _logger.LogError(ex.Message, ex);
        }
    }


    public IModel GetChannel()
     => _channel;

}
