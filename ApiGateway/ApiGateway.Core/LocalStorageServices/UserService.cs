using System.Threading.Tasks;

namespace ApiGateway.Core.LocalStorageServices
{
    public class UserService : IUserService
    {
        private readonly ILocalStorageService _localStorageService;


        public UserService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<User> GetCurrentUser()
        {
            var user = await _localStorageService.GetItem<User>("user");
            return user;
        }
    }
}
