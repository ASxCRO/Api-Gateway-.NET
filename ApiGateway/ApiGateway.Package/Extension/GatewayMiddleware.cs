using ApiGateway.Package.Helpers;
using ApiGateway.Package.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace ApiGateway.Package.Extension
{
    public class GatewayMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RateLimitingCache rateLimitingCache;
        private readonly IConfiguration configuration;

        public GatewayMiddleware(RequestDelegate next, RateLimitingCache rateLimitingCache, IConfiguration configuration)
        {
            _next = next;
            this.rateLimitingCache = rateLimitingCache;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Router router = new Router("routes.json", rateLimitingCache, configuration);
            var response = await router.RouteRequest(httpContext.Request, httpContext.Connection.RemoteIpAddress.MapToIPv4());
            httpContext.Response.StatusCode = (int)response.StatusCode;
            await httpContext.Response.WriteAsync(await response.Content.ReadAsStringAsync());
        }
    }
}
