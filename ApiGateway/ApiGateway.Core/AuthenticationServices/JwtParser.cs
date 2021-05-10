using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ApiGateway.Core.AuthenticationServices
{
    public static class JwtParser
    {
        public static string[] ExtractUserAccessModules(ClaimsPrincipal claims)
        {
            var moduleClaims = claims.Identities.FirstOrDefault().Claims.Where(c => c.Type.Equals("exampleClaim")).ToList();
            List<string> modules = new();
            foreach (var module in moduleClaims)
            {
                modules.Add(module.Value);
            }

            return modules.ToArray();
        }

        public static string[] ExtractUserRoles(ClaimsPrincipal claims)
        {
            var roleClaims = claims.Identities.FirstOrDefault().Claims.Where(c => c.Type.Equals(ClaimTypes.Role)).ToList();
            List<string> roles = new();
            foreach (var role in roleClaims)
            {
                roles.Add(role.Value);
            }
            return roles.ToArray();
        }

        public static string ExtractUserIdentifier(ClaimsPrincipal claims)
        {
            var userIdentifierClaim = claims.Identities.FirstOrDefault().Claims.Where(c => c.Type.Equals(ClaimTypes.NameIdentifier)).FirstOrDefault();
            return userIdentifierClaim.Value;
        }

        public static IEnumerable<Claim> ExtractClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = false,
                ValidateLifetime = false,
                SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                {
                    var jwt = new JwtSecurityToken(token);
                    return jwt;
                }
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

            return claims.Claims;
        }

        public static ClaimsPrincipal ExtractClaimsPrincipal(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = false,
                ValidateLifetime = false,
                SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                {
                    var jwt = new JwtSecurityToken(token);

                    return jwt;
                }
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


    }
}
