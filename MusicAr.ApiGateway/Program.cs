using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "MusicArAuthService",
            ValidAudience = "MusicArClients",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("uirW30wVdDjrQl2q1jfkMbPdup4q8hmJOmGjJgcGsqVyK5REWTWta7jtX7hQ7unh"))
        };
    });

builder.Services.AddReverseProxy()
    .LoadFromConfig(
        builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddControllers();
var app = builder.Build();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapReverseProxy();
app.MapControllers();
app.Run();
