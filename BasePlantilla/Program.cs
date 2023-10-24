using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Auth;
using Models.Helpers;
using Services.AuthServices;
using Services.HomeServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager contiguration = builder.Configuration;

// Add services to the container.
builder.Services.AddTransient<IHomeService, HomeService>();
builder.Services.AddTransient<IAuthService, AuthService>();

// Add middleware to the HTTP request pipeline.
builder.Services.AddScoped<RenovarToken>(); // middleware para proporcionar el token de refresco

// Entity Framework
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(contiguration.GetConnectionString("ConnStr")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // configurar validaciones de token
        .AddJwtBearer(options => {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(contiguration["JWT:Secret"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();// autenticar peticiones
app.UseAuthorization();

app.MapControllers();

app.Run();
