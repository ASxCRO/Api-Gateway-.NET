using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Package.Models
{
    public class RateLimit
    {
        public string IPAddress { get; set; }
        public int ServiceId { get; set; }
        public int RatePerDay { get; set; }
    }
}
