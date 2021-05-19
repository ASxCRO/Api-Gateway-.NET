using ApiGateway.Package.Hash;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Package.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BaseUri { get; set; }
        public List<string> IPSafelist { get; set; }
    }
}
