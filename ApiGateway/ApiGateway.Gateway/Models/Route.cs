using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Gateway.Models
{
    public class Route
    {
        public string Endpoint { get; set; }
        public Destination Destination { get; set; }
    }
}
