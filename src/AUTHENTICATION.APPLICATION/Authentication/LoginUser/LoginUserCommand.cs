using MediatR;

namespace AUTHENTICATION.APPLICATION.Authentication.LoginUser;

public record LoginUserCommand(string UserName, string PassWord) : IRequest<LoginUserResponse>;
