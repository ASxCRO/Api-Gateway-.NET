using ApiGateway.Core.RequestModels;
using ApiGateway.Core.User;
using System.Threading.Tasks;

namespace ApiGateway.Core.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task Initialize();
        Task<bool> Login(LoginRequest loginRequest);
        Task Logout();
    }
}
