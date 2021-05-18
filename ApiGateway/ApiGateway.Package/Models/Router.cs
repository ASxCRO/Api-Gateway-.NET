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

        public Router(string routeConfigFilePath)
        {
            dynamic router = JsonLoader.LoadFromFile<dynamic>(routeConfigFilePath);
            Routes = JsonLoader.Deserialize<List<Route>>(Convert.ToString(router.routes));
            Services = JsonLoader.Deserialize<List<Service>>(Convert.ToString(router.services));
        }

        public async Task<HttpResponseMessage> RouteRequest(HttpRequest request)
        {
            string path = request.Path.ToString();
            string basePath = '/' + path.Split('/')[1];

            Destination destination;
            Service service;
            try
            {
                destination = Routes.First(r => r.Endpoint.Equals(basePath)).Destination;
                service = Services.First(s => s.Id.Equals(destination.ServiceId));
                destination.Uri = $"{service.baseUri}/{destination.Uri}";
            }
            catch
            {
                return ConstructErrorMessage("Nije moguće pronaći putanju.");
            }

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
    }
}
