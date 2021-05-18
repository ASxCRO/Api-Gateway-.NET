using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Package.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string baseUri { get; set; }
    }
}
