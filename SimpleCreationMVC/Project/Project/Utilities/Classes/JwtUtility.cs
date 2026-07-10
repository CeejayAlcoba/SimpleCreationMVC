
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Models;
using Utilities.Interfaces;

namespace Utilities.Classes
{
    public class JwtUtility : IJwtUtility
    {
        private readonly IConfigurationRoot _config;

        public JwtUtility(IAppUtility appUtility)
        {
            _config = appUtility.GetConfiguration();
        }

        public string GenerateToken(int userId)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", userId.ToString() ?? "")
            };

            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
