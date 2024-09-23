using AUTHENTICATION.DOMAIN;
using AUTHENTICATION.INFRA.RabbitMq;
using AUTHENTICATION.INFRA.RabbitMq.model;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AUTHENTICATION.APPLICATION.Authentication.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IPublishRabbitMqService _publishRabbitMqService;

    public CreateUserCommandHandler(UserManager<User> userManager, IPublishRabbitMqService publishRabbitMqService)
    {
        _userManager = userManager;
        _publishRabbitMqService = publishRabbitMqService;
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = this.ConverterUser(request);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(request.Crm))
                _publishRabbitMqService.Publish<EventCreateDoctorModel>(EventCreateDoctorModel.ToEvent(user, request.Crm));
            else
                _publishRabbitMqService.Publish<EventCreatePatientModel>(EventCreatePatientModel.ToEvent(user));

            return CreateUserResponse.ToResponse(user);


        }
        throw new Exception(result.Errors.ToString());
    }

    private User ConverterUser(CreateUserCommand command)
        => new User
        {
            Email = command.Email,
            Cpf = command.Cpf,
            UserName = command.Name,
        };
}
