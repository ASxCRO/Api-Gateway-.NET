using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.API.Data.Database.Models
{
    public class Adresa
    {
        public int Id { get; set; }
        public string Polaziste{ get; set; }
        public string Odrediste { get; set; }
        public double LatPolaziste{ get; set; }
        public double LngPolaziste { get; set; }
        public double LatOdrediste { get; set; }
        public double LngOdrediste { get; set; }
        public double Vrijeme{ get; set; }
        public double Distanca { get; set; }
    }
}
