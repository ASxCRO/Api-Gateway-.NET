using ApiGateway.Package.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.Package.Models
{
    public class Router
    {
        public List<Route> Routes { get; set; }
        public List<Service> Services { get; set; }
        public List<RateLimit> RateLimits { get; set; }
        private RateLimitingCache _rateLimitingCache { get; }

        public Router(string routeConfigFilePath, RateLimitingCache rateLimitingCache)
        {
            dynamic router = JsonLoader.LoadFromFile<dynamic>(routeConfigFilePath);
            Routes = JsonLoader.Deserialize<List<Route>>(Convert.ToString(router.routes));
            Services = JsonLoader.Deserialize<List<Service>>(Convert.ToString(router.services));
            _rateLimitingCache = rateLimitingCache;
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

                //if (destination.RequiresAuthentication)
                //{
                //    string token = request.Headers["Authorization"];
                //    request.Query.Append(new KeyValuePair<string, StringValues>("bearer", new StringValues(token)));
                //    HttpResponseMessage authResponse = await AuthenticationService.SendRequest(request);
                //    if (!authResponse.IsSuccessStatusCode) return ConstructErrorMessage("Neautorizirani pristup.");
                //}

                return await destination.SendRequest(request);
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

        public int GetRateLimit()
        {
            return 0;
        }

    }
}
