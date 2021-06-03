using ApiGateway.Core.Models.RequestModels;
using ApiGateway.Core.RequestModels;
using ApiGateway.Core.ResponseModels;
using System.Threading.Tasks;

namespace ApiGateway.Core.AuthenticationServices
{
    public interface IAuthenticationService
    {
        LoginResponse User { get; set; }
        Task Initialize();
        Task<bool> Login(LoginRequest loginRequest);
        Task<bool> Register(RegisterRequest registerRequest);
        Task<bool> Update(User.User user);
        Task<User.User> GetById(int id);
        Task Logout();
    }
}
