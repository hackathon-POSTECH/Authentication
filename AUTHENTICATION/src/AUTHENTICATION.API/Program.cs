using AUTHENTICATION.API.Configuration;
using AUTHENTICATION.APPLICATION.Authentication.CreateUser;
using AUTHENTICATION.DOMAIN;
using AUTHENTICATION.INFRA.context;
using AUTHENTICATION.INFRA.RabbitMq;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<AUTHENTICATIONCONTEXT>(options =>
      options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options => { options.SignIn.RequireConfirmedAccount = false; options.User.RequireUniqueEmail = false; })
    .AddEntityFrameworkStores<AUTHENTICATIONCONTEXT>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<ICreateChannelRabbitMql, CreateChannelRabbitMql>();
builder.Services.AddScoped<IPublishRabbitMqService, PublishRabbitMqService>();
builder.Services.AddInjectMediator();

builder.Services.AddHostedService<ConsumersRabbit>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("9ASHDA98H9ah9ha9H9A89n0fasjdhksajhduiwqadskjhkSKJD")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "AUTHENTICATION V1"); });

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
