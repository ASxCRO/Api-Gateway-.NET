using ApiGateway.Core.Models.RequestModels;
using ApiGateway.Core.RequestModels;
using AutoStoper.Authorization.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoStoper.Authorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(LoginRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Korisnicko ime / lozinka nisu ispravni" });

            return Ok(response);
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            var response = _userService.Register(model);

            if (response == null)
                return BadRequest(new { message = "Registracijske informacije nisu valjane" });

            return Ok(response);
        }

        [HttpPost("getall")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
