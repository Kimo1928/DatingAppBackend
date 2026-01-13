using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DatingAppWebApi.Entities
{
    public class User
    {
        public string Id { get; set; } 

        public DateOnly DateOfBirth { get; set; }

        public string? ImageUrl { get; set; }

        public required string DisplayName { get; set; }

        public DateTime Created { get; set; }=DateTime.UtcNow;

        public DateTime LastActive { get; set; }=DateTime.UtcNow;

        public required string Gender { get; set; }

        public required string City { get;set;}

        public string? Description { get; set; }
        public required string Country { get; set; }

        [JsonIgnore]
        public List<UserLike> LikedByUsers { get; set; } = [];

        [JsonIgnore]

        public List<UserLike> LikedUsers { get; set; } = [];



        [JsonIgnore]

        public virtual ICollection<Photo> Photos { get; set; } = [];

        [ForeignKey(nameof(Id))]
        public virtual AppUser AppUser { get; set; }=null!;




         
    }
}
