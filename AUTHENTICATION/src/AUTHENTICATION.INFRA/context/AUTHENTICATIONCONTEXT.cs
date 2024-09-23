using AUTHENTICATION.DOMAIN;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AUTHENTICATION.INFRA.context;

public class AUTHENTICATIONCONTEXT : IdentityDbContext<User>
{
    public AUTHENTICATIONCONTEXT(DbContextOptions<AUTHENTICATIONCONTEXT> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
