using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.API.Data.Database.Models
{
    public class VoznjaUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Vozac { get; set; }
        public int VoznjaId { get; set; }
        public virtual Voznja Voznja{ get; set; }
    }
}
