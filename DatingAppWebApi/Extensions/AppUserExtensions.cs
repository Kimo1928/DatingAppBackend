using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Interfaces;

namespace DatingAppWebApi.Extensions
{
    public static class AppUserExtensions 
    {
        public static UserDTO ToDto(this AppUser user, ITokenService tokenService) {

            return new UserDTO { DisplayName = user.DisplayName, Id = user.Id, Email = user.Email,ImageUrl=user.ImageUrl, Token = tokenService.CreateToken(user) } ;
        
        }
    }
}
