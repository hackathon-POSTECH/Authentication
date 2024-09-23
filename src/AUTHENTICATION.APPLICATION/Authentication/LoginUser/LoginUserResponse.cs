namespace AUTHENTICATION.APPLICATION.Authentication.LoginUser;

public class LoginUserResponse
{
    public string Token { get; set; }

    public static LoginUserResponse ToResponse(string token)
        => new LoginUserResponse
        {
            Token = token
        };
}
