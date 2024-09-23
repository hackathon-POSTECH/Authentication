using AUTHENTICATION.APPLICATION.Authentication.CreateUser;
using AUTHENTICATION.APPLICATION.Authentication.LoginUser;
using MediatR;

namespace AUTHENTICATION.API.Configuration;

public static class ConfigurationMediator
{
    public static IServiceCollection AddInjectMediator(this IServiceCollection services)
    {

        services.AddScoped<IMediator, Mediator>();
        services.AddTransient<IRequestHandler<CreateUserCommand, CreateUserResponse>, CreateUserCommandHandler>();
        services.AddTransient<IRequestHandler<LoginUserCommand, LoginUserResponse>, LoginUserCommandHandler>();

        return services;
    }
}
