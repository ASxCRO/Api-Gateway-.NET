using ApiGateway.Core.Services.AuthenticationServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        public async Task Invoke(HttpContext context/*, IUserService userService*/)
        {
            //var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.uDm - tzoBJuvceAuY8GsKeGOtf4NiZejy2rxyNv6p5Rw";

            if (token != null)
                attachUserToContext(context, token);

            await _next(context);
        }

        private async void attachUserToContext(HttpContext context/* IUserService userService*/, string token)
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
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                //var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "name").Value);

                // spoj korisnika na kontekst daje do znanja da je jwt autentikacija prosla
                //context.Items["User"] = userService.GetById(userId);
            }
            catch
            {
                // desava se kad autentikacija nije validna
                // korisnik nije spojen na kontekst stoga request nece imati prava na rute
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("jwt nije validan");
            }
        }
    }
}
