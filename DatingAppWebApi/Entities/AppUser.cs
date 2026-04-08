using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace DatingAppWebApi.Entities
{
    public class AppUser : IdentityUser
    {
  

        public required string DisplayName { get; set; }


        public string? ImageUrl { get; set; }


        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }



        [JsonIgnore]

        public virtual User? User { get; set; }


    }
}
