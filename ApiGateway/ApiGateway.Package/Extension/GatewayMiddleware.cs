using ApiGateway.Package.Helpers;
using ApiGateway.Package.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace ApiGateway.Package.Extension
{
    public class GatewayMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RateLimitingCache rateLimitingCache;

        public GatewayMiddleware(RequestDelegate next, RateLimitingCache rateLimitingCache)
        {
            _next = next;
            this.rateLimitingCache = rateLimitingCache;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Router router = new Router("routes.json", rateLimitingCache);
            var response = await router.RouteRequest(httpContext.Request, httpContext.Connection.RemoteIpAddress.MapToIPv4());
            httpContext.Response.StatusCode = (int)response.StatusCode;
            await httpContext.Response.WriteAsync(await response.Content.ReadAsStringAsync());
            await _next(httpContext);
        }
    }
}
