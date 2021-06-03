using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Core.Models.RequestModels
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Molimo unesite ime.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage= "Molimo unesite prezime.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage ="Molimo unesite ispravnu email adresu.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Molimo unesite korisničko ime.")]
        [MinLength(6,ErrorMessage = "Molimo unesite minimalno 6 znakova.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Molimo unesite lozinku.")]
        [MinLength(6, ErrorMessage = "Molimo unesite minimalno 6 znakova.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Molimo ponovno unesite lozinku.")]
        [Compare(nameof(Password), ErrorMessage = "Lozinke se ne podudaraju.")]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }

        public bool ActivateUser { get; set; } = false;
        public bool AutoConfirmEmail { get; set; } = false;
    }
}
