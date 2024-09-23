using Microsoft.Extensions.Hosting;

namespace AUTHENTICATION.INFRA.RabbitMq;

public class ConsumersRabbit : BackgroundService
{
    private readonly ICreateChannelRabbitMql _createChannelRabbitMql;
    public ConsumersRabbit(ICreateChannelRabbitMql createChannelRabbitMql)
    {
        _createChannelRabbitMql = createChannelRabbitMql;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.FromResult(0);
    }
}
