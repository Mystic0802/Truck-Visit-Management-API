using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Truck_Visit_Management_API.Authentication;

namespace Truck_Visit_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(IConfiguration config)
        {
            _tokenService = new TokenService(config);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            if (login.Username == "admin" && login.Password == "admin")
            {
                var token = _tokenService.GenerateToken(login);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid credentials");
        }
    }
}
