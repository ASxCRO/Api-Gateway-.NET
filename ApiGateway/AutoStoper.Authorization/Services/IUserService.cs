using ApiGateway.Core.Models.RequestModels;
using ApiGateway.Core.RequestModels;
using ApiGateway.Core.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStoper.Authorization.Services
{
    public interface IUserService
    {
        LoginResponse Authenticate(LoginRequest model);
        LoginResponse Register(RegisterRequest model);
        IEnumerable<ApiGateway.Core.User.User> GetAll();
        ApiGateway.Core.User.User Update(ApiGateway.Core.User.User user);
        ApiGateway.Core.User.User GetById(int id);
    }
}
