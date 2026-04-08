using DatingAppWebApi.Entities;
using DatingAppWebApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DatingAppWebApi.Services
{
    public class TokenService(IConfiguration config , UserManager<AppUser> userManager) : ITokenService
    {
        public async Task<string> CreateToken(AppUser user)
        {

            var tokenKey = config["TokenKey"]?? throw new Exception("Cannot get Token Key ");
            if (tokenKey.Length < 64) { throw new Exception("Token key length is less than 64 characters"); }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.DisplayName)

            };

            var roles = await userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role=> new Claim(ClaimTypes.Role,role)));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(7),
                SigningCredentials = creds ,


            };
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);  
        }

        public string GenerateRefreshToken()
        {
           var randonBytes=RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randonBytes);
        }
    }
}
