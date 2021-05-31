using ApiGateway.Core.Models.ResponseModels;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AutoStoper.Client.Pages
{
    public partial class Index
    {
        public List<Voznja> Voznje { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient("AutoStoper.Gateway"))
            {
                var response = await httpClient.GetAsync("voznje");
                if (response.IsSuccessStatusCode)
                    Voznje = await response.Content.ReadFromJsonAsync<List<Voznja>>();
            }
        }
    }
}
