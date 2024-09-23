using AUTHENTICATION.DOMAIN;

namespace AUTHENTICATION.INFRA.RabbitMq.model;

public class EventCreateDoctorModel
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Crm { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }

    public static RabbitMqPublishModel<EventCreateDoctorModel> ToEvent(User user, string Crm)
        => new RabbitMqPublishModel<EventCreateDoctorModel>()
        {
            ExchangeName = EventConstants.CREATE_DOCTOR_EXCHANGE,
            RoutingKey = "",
            Message = new EventCreateDoctorModel
            {
                UserId = Guid.Parse(user.Id),
                Cpf = user.Cpf,
                Name = user.UserName,
                Email = user.Email,
                Crm = Crm
            }
        };
}
