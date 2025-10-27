using DatingAppWebApi.Entities;

namespace DatingAppWebApi.Interfaces
{
    public interface ITokenService
    {
       public string CreateToken(AppUser user);
    }
}
