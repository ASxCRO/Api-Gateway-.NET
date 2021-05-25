using ApiGateway.Core.Services.AuthenticationServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Package.Extension
{
    public class JwtMiddleware
{
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration appSettings)
        {
            _next = next;
            _configuration = appSettings;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context,userService, token);

            await _next(context);
        }

        private async void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtOptions:Secret"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("jwt validan");
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                //spoj korisnika na kontext daje do znanja da je jwt autentikacija prosla
                context.Items["User"] = userService.GetById(userId);
            }
            catch(Exception ex)
            {
                // desava se kad autentikacija nije validna
                // korisnik nije spojen na kontekst stoga request nece imati prava na rute
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("jwt nije validan");
            }
        }
    }
}
