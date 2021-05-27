using ApiGateway.Core.RequestModels;
using ApiGateway.Core.ResponseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Core.Services.AuthenticationServices
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<ApiGateway.Core.User.User> _users = new List<ApiGateway.Core.User.User>
        {
            new ApiGateway.Core.User.User { Id = 1, FirstName = "Antonio", LastName = "Supan", Username = "test", Password = "test" }
        };

        private readonly IConfiguration _appSettings;

        public UserService(IConfiguration appSettings)
        {
            _appSettings = appSettings;
        }

        public LoginResponse Authenticate(LoginRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            if (user == null) 
                return null;

            var token = generateJwtToken(user);
            return new LoginResponse(user, token);
        }

        public IEnumerable<ApiGateway.Core.User.User> GetAll()
        {
            return _users;
        }

        public ApiGateway.Core.User.User GetById(string id)
        {
            return _users.FirstOrDefault(x => x.Id == Convert.ToInt32(id));
        }

        private string generateJwtToken(ApiGateway.Core.User.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings["JwtOptions:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
