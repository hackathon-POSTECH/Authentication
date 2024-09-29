using AUTHENTICATION.APPLICATION.Authentication.CreateUser;
using AUTHENTICATION.APPLICATION.Authentication.LoginUser;
using MediatR;
using System.Reflection;

namespace AUTHENTICATION.API.Configuration;

public static class ConfigurationMediator
{
    public static IServiceCollection AddInjectMediator(this IServiceCollection services)
    {

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddTransient<IRequestHandler<CreateUserCommand, CreateUserResponse>, CreateUserCommandHandler>();
        services.AddTransient<IRequestHandler<LoginUserCommand, LoginUserResponse>, LoginUserCommandHandler>();

        return services;
    }
}
