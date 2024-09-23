namespace AUTHENTICATION.INFRA.RabbitMq.model;

public class RabbitMqPublishModel<T>
{
    public string ExchangeName { get; set; }
    public string RoutingKey { get; set; }
    public T Message { get; set; }
}
