using AUTHENTICATION.DOMAIN;

namespace AUTHENTICATION.APPLICATION.Authentication.CreateUser;

public class CreateUserResponse
{

    public string Email { get; set; }
    public string PassWord { get; set; }
    public string UserName { get; set; }

    public static CreateUserResponse ToResponse(User user)
        => new CreateUserResponse
        {
            Email = user.Email,
            PassWord = user.PasswordHash,
            UserName = user.UserName,
        };

}
