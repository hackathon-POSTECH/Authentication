using AUTHENTICATION.DOMAIN;

namespace AUTHENTICATION.INFRA.RabbitMq.model;

public class EventCreateUserModel
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Crm { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }

    public static RabbitMqPublishModel<EventCreateUserModel> ToEvent(User user, string Crm)
        => new RabbitMqPublishModel<EventCreateUserModel>()
        {
            ExchangeName = EventConstants.CREATE_USER_EXCHANGE,
            RoutingKey = "",
            Message = new EventCreateUserModel
            {
                UserId = Guid.Parse(user.Id),
                Cpf = user.Cpf,
                Name = user.UserName,
                Email = user.Email,
                Crm = Crm
            }
        };
}
