using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.AuthServices;
using System.Security.Claims;

namespace Models.Helpers {
    public class RenovarToken : IMiddleware {
        //private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public RenovarToken( IConfiguration configuration, IAuthService auth) {
            
            _configuration = configuration;
            _authService = auth;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next) {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null) {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
                try {
                    var tokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                    var validoHasta = validatedToken.ValidTo.Subtract(DateTime.UtcNow);
                    var fromminutes = TimeSpan.FromMinutes(5);
                    if (validoHasta <= TimeSpan.Zero) {
                        // El token ha expirado, devolver una respuesta de error 401 no autorizado
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    } else if (validoHasta <= fromminutes) {
                        var mail = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value)
                            .FirstOrDefault();
                        if (mail == null) {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        }
                        //var _authService = context.RequestServices.GetService(typeof(IAuthService)) as IAuthService;
                        var user = _authService.ObtenerUsuario(mail);
                        var newToken = _authService.GenerarToken(user);
                        context.Response.Headers.Add("Authorization", "Bearer " + newToken);
                    }
                } catch (Exception ex) {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
            }
            return next(context);
        }
    }
}
