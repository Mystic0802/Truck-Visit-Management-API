using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Truck_Visit_Management_API.Authentication
{
    public class TokenService
    {
        private readonly string _key;

        public TokenService(IConfiguration config)
        {
            _key = config["AuthSecret"];
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JsonWebTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            var securityKey = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username)
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = signingCredentials,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
