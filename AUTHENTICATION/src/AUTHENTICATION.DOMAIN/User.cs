using Microsoft.AspNetCore.Identity;

namespace AUTHENTICATION.DOMAIN;

public class User : IdentityUser
{
    public string Cpf { get; set; }
}
