using ApiGateway.Core.User;
using System.Threading.Tasks;
using ApiGateway.Core.ResponseModels;


namespace ApiGateway.Core.LocalStorageServices
{
    public class UserService : IUserService
    {
        private readonly ILocalStorageService _localStorageService;


        public UserService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<LoginResponse> GetCurrentUser()
        {
            var user = await _localStorageService.GetItem<LoginResponse>("user");
            return user;
        }
    }
}
