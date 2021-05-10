using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace ApiGateway.Core.AuthenticationServices
{
    public static class TokenHelper
    {
        public static ClaimsPrincipal ExtractClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = false,
                ValidateLifetime = false
            };
            ClaimsPrincipal claims;
            try
            {
                claims = handler.ValidateToken(token, validations, out SecurityToken tokenSecure);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return claims;
        }

        public static string ValidateToken(string token, string secretKey)
        {
            var key = Encoding.ASCII.GetBytes(secretKey);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
            };

            try
            {
                var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            }
            catch (Exception)
            {
                throw;
            }

            return token;
        }

        public static string GetCookieFromHeader(HttpResponseMessage message)
        {
            message.Headers.TryGetValues("Set-Cookie", out var setCookie);
            var setCookieString = setCookie.Single();
            var cookieTokens = setCookieString.Split(';');
            var firstCookie = cookieTokens.FirstOrDefault();
            var keyValueTokens = firstCookie.Split('=');
            var valueString = keyValueTokens[1];
            var cookieValue = HttpUtility.UrlDecode(valueString);
            return cookieValue;
        }

        public static List<string> ExtractUserRoles(ClaimsPrincipal claims)
        {
            var roleClaims = claims.Identities.FirstOrDefault().Claims.Where(c => c.Type.Equals(ClaimTypes.Role)).ToList();
            List<string> roles = new();
            foreach (var role in roleClaims)
            {
                roles.Add(role.Value);
            }
            return roles;
        }

        public static string ExtractUserIdentifier(ClaimsPrincipal claims)
        {
            var userIdentifierClaim = claims.Identities.FirstOrDefault().Claims.Where(c => c.Type.Equals(ClaimTypes.NameIdentifier)).FirstOrDefault();
            return userIdentifierClaim.Value;
        }



    }
}
