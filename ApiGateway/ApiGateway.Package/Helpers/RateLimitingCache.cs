using ApiGateway.Package.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Package.Helpers
{
    public class RateLimitingCache
    {
        public RateLimitingCache()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(1);

            var timer = new System.Threading.Timer((e) =>
            {
                dynamic router = JsonLoader.LoadFromFile<dynamic>("routes.json");
                AppRateLimits = JsonLoader.Deserialize<List<RateLimit>>(Convert.ToString(router.rateLimiting));
            }, null, startTimeSpan, periodTimeSpan);
        }

        public List<RateLimit> AppRateLimits{ get; set; }
        public object Timespan { get; }
    }
}
