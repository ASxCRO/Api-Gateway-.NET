using ApiGateway.Core.Models.ResponseModels;
using ApiGateway.Core.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Client.ViewModels
{
    public class VoznjaViewModel
    {
        public Voznja Voznja { get; set; }
        public List<User> Putnici { get; set; }

    }
}
