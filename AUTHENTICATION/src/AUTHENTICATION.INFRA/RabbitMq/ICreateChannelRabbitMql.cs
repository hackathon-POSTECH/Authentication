using RabbitMQ.Client;

namespace AUTHENTICATION.INFRA.RabbitMq;

public interface ICreateChannelRabbitMql
{
    IModel GetChannel();
}
