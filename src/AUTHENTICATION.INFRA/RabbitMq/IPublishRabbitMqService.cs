using AUTHENTICATION.INFRA.RabbitMq.model;

namespace AUTHENTICATION.INFRA.RabbitMq;

public interface IPublishRabbitMqService
{
    void Publish<T>(RabbitMqPublishModel<T> rabbitMqConfig);
}
