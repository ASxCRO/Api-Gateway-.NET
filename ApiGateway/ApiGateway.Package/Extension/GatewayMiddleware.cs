using ApiGateway.Package.Helpers;
using ApiGateway.Package.Logging;
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
        private readonly Logger logger;

        public GatewayMiddleware(RequestDelegate next, RateLimitingCache rateLimitingCache, IConfiguration configuration, Logger logger)
        {
            _next = next;
            this.rateLimitingCache = rateLimitingCache;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Router router = new Router("routes.json", rateLimitingCache, configuration, logger);
            var response = await router.RouteRequest(httpContext.Request, httpContext.Connection.RemoteIpAddress.MapToIPv4());
            httpContext.Response.StatusCode = (int)response.StatusCode;
            await httpContext.Response.WriteAsync(await response.Content.ReadAsStringAsync());
        }
    }
}
