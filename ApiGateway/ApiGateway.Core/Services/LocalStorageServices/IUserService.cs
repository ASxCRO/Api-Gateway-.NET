using ApiGateway.Core.User;
using System.Threading.Tasks;

namespace ApiGateway.Core.LocalStorageServices
{
    public interface IUserService
    {
        Task<User.User> GetCurrentUser();
    }
}
