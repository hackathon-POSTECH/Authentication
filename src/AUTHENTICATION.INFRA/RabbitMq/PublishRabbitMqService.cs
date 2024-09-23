using AUTHENTICATION.INFRA.RabbitMq.model;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AUTHENTICATION.INFRA.RabbitMq;

public class PublishRabbitMqService : IPublishRabbitMqService
{
    private readonly IModel _channel;
    private readonly ILogger<PublishRabbitMqService> _logger;
    public PublishRabbitMqService(ICreateChannelRabbitMql createChannelRabbitMql, ILogger<PublishRabbitMqService> logger)
    {
        _channel = createChannelRabbitMql.GetChannel();
        _logger = logger;
    }

    public void Publish<T>(RabbitMqPublishModel<T> rabbitMqConfig)
    {
        try
        {
            var body = EncodingMessage(rabbitMqConfig.Message);
            _channel.BasicPublish(
                rabbitMqConfig.ExchangeName,
                rabbitMqConfig.RoutingKey,
                false,
                null,
                body
            );
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while publish RabbitMq.");
            _logger.LogError(ex.Message, ex);
        }
    }
    private byte[] EncodingMessage<T>(T message)
    {
        var jsonSerialize = JsonSerializer.Serialize(message);
        return Encoding.UTF8.GetBytes(jsonSerialize);
    }

}
