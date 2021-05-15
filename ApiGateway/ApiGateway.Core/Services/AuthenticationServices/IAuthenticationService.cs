using ApiGateway.Core.User;
using System.Threading.Tasks;

namespace ApiGateway.Core.AuthenticationServices
{
    public interface IAuthenticationService
    {
        AppUser User { get; }
        Task Initialize();
        Task<bool> Login(string username, string password);
        Task Logout();
        Task<AppUser> ValidateToken(AppUser user);
    }
}
