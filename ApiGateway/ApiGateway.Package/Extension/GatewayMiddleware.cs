using ApiGateway.Package.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ApiGateway.Package.Extension
{
    public class GatewayMiddleware
    {
        private readonly RequestDelegate _next;

        public GatewayMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            Router router = new Router("routes.json");
            var response = await router.RouteRequest(httpContext.Request);
            var content = await response.Content.ReadAsStringAsync();
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(content);
            await _next(httpContext);
        }
    }
}
