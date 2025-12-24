using System.Security.Claims;

namespace DatingAppWebApi.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier)?? 
                throw new Exception("Can't get UserId from token ") ;
        }
    }
}
