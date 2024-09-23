using MediatR;

namespace AUTHENTICATION.APPLICATION.Authentication.CreateUser;

public record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    string Cpf,
    string? Crm) : IRequest<CreateUserResponse>;