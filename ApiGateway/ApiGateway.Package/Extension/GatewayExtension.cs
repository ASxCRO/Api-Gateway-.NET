using Microsoft.AspNetCore.Builder;

namespace ApiGateway.Package.Extension
{
    public static class GatewayExtension
    {
        public static IApplicationBuilder UseApiGateway(this IApplicationBuilder app)
        {
            app.UseMiddleware<GatewayMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseApiGatewayAuthorization(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseApiKeyValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiKeyMiddleware>();
            return app;
        }
    }
}
