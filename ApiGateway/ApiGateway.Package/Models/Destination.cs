using ApiGateway.Package.Hash;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Package.Models
{
    public class Destination
    {
        public int ServiceId{ get; set; }
        public string Uri { get; set; }
        public bool RequiresAuthentication { get; set; }

        public Destination(string uri, bool requiresAuthentication)
        {
            Uri = uri;
            RequiresAuthentication = requiresAuthentication;
        }

        public Destination(string uri)
            : this(uri, false)
        {
        }

        private Destination()
        {
            Uri = "/";
            RequiresAuthentication = false;
        }


        private string CreateDestinationUri(HttpRequest request)
        {
            string requestPath = request.Path.ToString();
            string queryString = request.QueryString.ToString();

            string endpoint = "";
            string[] endpointSplit = requestPath.Substring(1).Split('/');

            if (endpointSplit.Length > 1)
                endpoint = endpointSplit[1];

            return Uri + endpoint + queryString;
        }


        public async Task<HttpResponseMessage> SendRequest(HttpRequest request, string apiKeySecret)
        {
            var datum = DateTime.Now.ToString();
            var dateBytes = Encoding.UTF8.GetBytes(datum);
            var saltBytes = Encoding.ASCII.GetBytes(apiKeySecret);
            var hash = Hashinator.GenerateSaltedHash(dateBytes, saltBytes);
            var hashBase64 = Convert.ToBase64String(hash);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Datum", datum);
                client.DefaultRequestHeaders.Add("Hash", hashBase64);

                var newRequest = new HttpRequestMessage(new HttpMethod(request.Method), CreateDestinationUri(request));
                if (request.Method == "POST")
                {
                    try
                    {
                        var reader = new StreamReader(request.Body);
                        var rawMessage = await reader.ReadToEndAsync();
                        newRequest.Content = new StringContent(rawMessage, Encoding.UTF8, "application/json");
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
                
                return await client.SendAsync(newRequest);
            }
        }
    }
}
