using AUTHENTICATION.DOMAIN;
using AUTHENTICATION.INFRA.context;
using AUTHENTICATION.INFRA.RabbitMq;
using AUTHENTICATION.INFRA.RabbitMq.model;
using MediatR;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;

namespace AUTHENTICATION.APPLICATION.Authentication.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IPublishRabbitMqService _publishRabbitMqService;
    private readonly AUTHENTICATIONCONTEXT _context;

    public CreateUserCommandHandler(UserManager<User> userManager, IPublishRabbitMqService publishRabbitMqService, AUTHENTICATIONCONTEXT context)
    {
        _context = context;
        _userManager = userManager;
        _publishRabbitMqService = publishRabbitMqService;
    }

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var user = this.ConverterUser(request);
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    _publishRabbitMqService.Publish<EventCreateUserModel>(EventCreateUserModel.ToEvent(user, request.Crm));
                    transaction.Commit();
                    return CreateUserResponse.ToResponse(user);


                }
                throw new Exception(result.Errors.ToString());
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw new Exception(e.Message);
            }
        }
    }

    private User ConverterUser(CreateUserCommand command)
        => new User
        {
            Email = command.Email,
            Cpf = command.Cpf,
            UserName = command.Name,
        };
}
