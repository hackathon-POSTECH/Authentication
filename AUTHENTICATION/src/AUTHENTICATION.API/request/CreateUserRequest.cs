using AUTHENTICATION.APPLICATION.Authentication.CreateUser;

namespace AUTHENTICATION.API.request;

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public string PassWord { get; set; }
    public string? Crm { get; set; }


    public CreateUserCommand ToCommand()
        => new CreateUserCommand(Name, Email, PassWord, Cpf, Crm);
}
