using ApiGateway.Core.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.API.Data.Database.Models
{
    public class Voznja
    {
        public int Id { get; set; }
        public bool LjubimciDozvoljeni { get; set; }
        public bool PusenjeDozvoljeno { get; set; }
        public bool AutomatskoOdobrenje { get; set; }
        public int MaksimalnoPutnika { get; set; }
        public DateTime DateTime { get; set; }
        public int AdresaId { get; set; }
        public double Cijena { get; set; }
        public virtual Adresa Adresa { get; set; }
        public virtual ICollection<VoznjaUser> Putnici { get; set; }

    }
}
