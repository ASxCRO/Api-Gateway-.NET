using System.Text.Json.Serialization;

namespace ApiGateway.Core.User
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? JwtToken { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Image { get; set; }
        public string Password { get; set; }
    }
}
