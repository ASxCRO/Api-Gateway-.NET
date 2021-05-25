using ApiGateway.Core.User;
using System.Threading.Tasks;

namespace ApiGateway.Core.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task Initialize();
        Task<bool> Login(string username, string password);
        Task Logout();
    }
}
