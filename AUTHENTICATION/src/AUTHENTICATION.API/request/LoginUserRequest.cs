using AUTHENTICATION.APPLICATION.Authentication.LoginUser;

namespace AUTHENTICATION.API.request;

public class LoginUserRequest
{
    public string UserName { get; set; }
    public string PassWord { get; set; }

    public LoginUserCommand ToCommand()
        => new LoginUserCommand(UserName, PassWord);
}
