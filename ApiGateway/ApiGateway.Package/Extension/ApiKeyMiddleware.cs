using ApiGateway.Package.Hash;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
                var dateBytes = Encoding.ASCII.GetBytes(datum);
                var saltBytes = Encoding.ASCII.GetBytes(salt);
                var hash = Hashinator.GenerateSaltedHash(dateBytes, saltBytes);
                var hashString = Encoding.ASCII.GetString(hash);

                var areHashesTheSame = Hashinator.CompareStrings(hashString, hashRequestString);
                if(areHashesTheSame)
                {
                    await _next(httpContext);
                }

            }

            httpContext.Abort();
        }
    }
}