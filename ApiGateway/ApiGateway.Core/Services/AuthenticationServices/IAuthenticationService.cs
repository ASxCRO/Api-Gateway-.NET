using ApiGateway.Core.RequestModels;
using ApiGateway.Core.ResponseModels;
using ApiGateway.Core.User;
using System.Threading.Tasks;

namespace ApiGateway.Core.AuthenticationServices
{
    public interface IAuthenticationService
    {
        LoginResponse User { get; set; }
        Task Initialize();
        Task<bool> Login(LoginRequest loginRequest);
        Task Logout();
    }
}
