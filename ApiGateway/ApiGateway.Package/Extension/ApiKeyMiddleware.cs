using ApiGateway.Package.Hash;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Package.Extension
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration appSettings;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration appSettings)
        {
            _next = next;
            this.appSettings = appSettings;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            bool isDatumHere = httpContext.Request.Headers.TryGetValue("Datum", out var datum);
            bool isHashHere = httpContext.Request.Headers.TryGetValue("Hash", out var hashRequestString);

            if(isDatumHere && isHashHere)
            {
                var salt = appSettings["ApiKeyOptions:Secret"];
                var dateBytes = Encoding.UTF8.GetBytes(datum);
                var saltBytes = Encoding.UTF8.GetBytes(salt);
                var hash = Hashinator.GenerateSaltedHash(dateBytes, saltBytes);
                var hashBase64 = Convert.ToBase64String(hash);

                var areHashesTheSame = Hashinator.CompareStrings(hashBase64, hashRequestString);
                if(areHashesTheSame)
                {
                    await _next(httpContext);
                }
                else
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Hash nije validan");
                }
            }
            else
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Nedostaju polja u headeru.");
            }
            

        }
    }
}