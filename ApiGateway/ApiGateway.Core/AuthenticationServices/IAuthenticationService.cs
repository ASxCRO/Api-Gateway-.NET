using System.Threading.Tasks;

namespace ApiGateway.Core.AuthenticationServices
{
    public interface IAuthenticationService
    {
        User User { get; }
        Task Initialize();
        Task<bool> Login(string username, string password);
        Task Logout();
        Task<User> ValidateToken(User user);
    }
}
