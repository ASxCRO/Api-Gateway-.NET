using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Core.Models
{
    public class Ruta
    {
        public string Polaziste { get; set; }
        public string Odrediste { get; set; }
        public double Distanca{ get; set; }
        public double Vrijeme { get; set; }
    }
}
