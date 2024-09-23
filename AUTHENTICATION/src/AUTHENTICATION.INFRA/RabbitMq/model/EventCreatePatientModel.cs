using AUTHENTICATION.DOMAIN;

namespace AUTHENTICATION.INFRA.RabbitMq.model;

public class EventCreatePatientModel
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }

    public static RabbitMqPublishModel<EventCreatePatientModel> ToEvent(User user)
       => new RabbitMqPublishModel<EventCreatePatientModel>()
       {
           ExchangeName = EventConstants.CREATE_PATIENT_EXCHANGE,
           RoutingKey = "",
           Message = new EventCreatePatientModel
           {
               UserId = Guid.Parse(user.Id),
               Cpf = user.Cpf,
               Name = user.UserName,
               Email = user.Email,
           }
       };

}
