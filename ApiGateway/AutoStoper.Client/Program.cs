using ApiGateway.Core.AuthenticationServices;
using ApiGateway.Core.HttpServices;
using ApiGateway.Core.LocalStorageServices;
using ApiGateway.Core.Services.AuthenticationServices;
using ApiGateway.Core.SpinnerServices;
using AutoStoper.Client.Shared.Preferences;
using Blazor.AdminLte;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ApiGateway.Core.Models.Enums;

namespace AutoStoper.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            var http = new HttpClient()
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            };

            builder.Services.AddScoped(sp => http);
            builder.Services.AddAdminLte();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddScoped<IWebAssemblyHttpService, WebassemblyHttpService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<SpinnerService>();

            builder.Services.AddMudServices();

            using var response = await http.GetAsync("appsettings.json");
            using var stream = await response.Content.ReadAsStreamAsync();
            builder.Configuration.AddJsonStream(stream);

            var gatewayConnectionString = builder.Configuration["APIConnectionStrings:Gateway"];

            builder.Services.AddHttpClient(ApiGateway.Core.Models.Enums.Client.ApiGateway, client => {
                client.BaseAddress = new Uri(gatewayConnectionString);
            });

            var host = builder.Build();

            var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
            await authenticationService.Initialize();

            await host.RunAsync();

        }
    }
}
