using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Core.Models.ResponseModels
{
    public class VoznjaUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VozacId { get; set; }
        public int VoznjaId { get; set; }
        public virtual Voznja Voznja{ get; set; }
    }
}
