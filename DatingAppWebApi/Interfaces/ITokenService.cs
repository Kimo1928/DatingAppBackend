using DatingAppWebApi.Entities;

namespace DatingAppWebApi.Interfaces
{
    public interface ITokenService
    {
       public Task<string> CreateToken(AppUser user);


        public string GenerateRefreshToken();
    }
}
