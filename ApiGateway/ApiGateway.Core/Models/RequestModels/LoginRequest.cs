using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Core.RequestModels
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Unesite korisničko ime")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Unesite lozinku")]
        public string Password { get; set; }
    }
}
