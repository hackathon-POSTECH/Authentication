using AUTHENTICATION.DOMAIN;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AUTHENTICATION.APPLICATION.Authentication.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public LoginUserCommandHandler(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var verifyPassWord = await _signInManager.PasswordSignInAsync(request.UserName, request.PassWord, false, false);
            if (!verifyPassWord.Succeeded) throw new Exception();
            var user = await _userManager.FindByNameAsync(request.UserName);
            return LoginUserResponse.ToResponse(GenerateToken(user));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private string GenerateToken(User user)
    {
        Claim[] claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim("Id", user.Id),

                };
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9ASHDA98H9ah9ha9H9A89n0fasjdhksajhduiwqadskjhkSKJD"));

        var signingCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            expires: DateTime.Now.AddDays(1),
            claims: claims,
            signingCredentials: signingCredentials
            );
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;
    }
}
