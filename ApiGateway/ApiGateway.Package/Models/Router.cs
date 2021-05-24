using ApiGateway.Package.Hash;
using ApiGateway.Package.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Package.Logging;

namespace ApiGateway.Package.Models
{
    public class Router
    {
        private readonly IConfiguration configuration;
        private readonly Logger logger;

        public List<Route> Routes { get; set; }
        public List<Service> Services { get; set; }
        public List<RateLimit> RateLimits { get; set; }
        private RateLimitingCache _rateLimitingCache { get; }

        public Router(string routeConfigFilePath, RateLimitingCache rateLimitingCache, IConfiguration configuration, Logger logger)
        {
            dynamic router = JsonLoader.LoadFromFile<dynamic>(routeConfigFilePath);
            Routes = JsonLoader.Deserialize<List<Route>>(Convert.ToString(router.routes));
            Services = JsonLoader.Deserialize<List<Service>>(Convert.ToString(router.services));
            _rateLimitingCache = rateLimitingCache;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<HttpResponseMessage> RouteRequest(HttpRequest request,IPAddress  iPAddress)
        {
            string path = request.Path.ToString();
            string basePath = '/' + path.Split('/')[1];

            Destination destination;
            Service service;
            try
            {
                destination = Routes.First(r => r.Endpoint.Equals(basePath)).Destination;
                service = Services.First(s => s.Id.Equals(destination.ServiceId));
                destination.Uri = $"{service.BaseUri}/{destination.Uri}";
            }
            catch
            {
                return ConstructErrorMessage("Nije moguće pronaći putanju.");
            }

            if (!service.IPSafelist.Contains(iPAddress.ToString()))
                return ConstructErrorMessage($"Nije moguće pristupiti uslugama sa IP adrese {iPAddress.MapToIPv4()}");

            var rateLimit = _rateLimitingCache.AppRateLimits.FirstOrDefault(r => r.IPAddress.Equals(iPAddress.MapToIPv4().ToString()) && r.ServiceId.Equals(service.Id));
            if (rateLimit is not null)
                if (rateLimit.RatePerDay > 0)
                    rateLimit.RatePerDay--;
                else
                    return ConstructErrorMessage($"Iskoristili ste {service.Name} za danas.");

            if (destination.RequiresAuthentication)
            {
                string token = request.Headers["Authorization"];
                request.Query.Append(new KeyValuePair<string, StringValues>("bearer", new StringValues(token)));
                var authService = Services.First(s => s.Name.Equals("authenticationService"));
                using(var HttpClient = new HttpClient())
                {
                    var datum = DateTime.Now.ToString();
                    var dateBytes = Encoding.UTF8.GetBytes(datum);
                    var saltBytes = Encoding.ASCII.GetBytes(configuration["ApiKeyOptions:Secret"]);
                    var hash = Hashinator.GenerateSaltedHash(dateBytes, saltBytes);
                    var hashBase64 = Convert.ToBase64String(hash);

                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Datum", datum);
                        client.DefaultRequestHeaders.Add("Hash", hashBase64);

                        var newRequest = new HttpRequestMessage(new HttpMethod(request.Method), $"{authService.BaseUri}");
                        var authResponse = await client.SendAsync(newRequest);
                        if (!authResponse.IsSuccessStatusCode) return ConstructErrorMessage("Neautorizirani pristup.");
                    }
                }

                logger.Log($"Korisnik sa tokenom {token} i IP adresom {iPAddress.MapToIPv4().ToString()} uputio je zahtjev na endpoint {basePath} kako bi dohvatio podatke sa {destination.Uri} datuma {DateTime.Now.ToString()}");
            }




            return await destination.SendRequest(request, configuration["ApiKeyOptions:Secret"]);
        }

        private HttpResponseMessage ConstructErrorMessage(string error)
        {
            HttpResponseMessage errorMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(error)
            };
            return errorMessage;
        }
    }
}
