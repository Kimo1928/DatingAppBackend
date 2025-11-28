using System.Text.Json.Serialization;

namespace DatingAppWebApi.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; }

        public string? PublicId { get; set; }



        [JsonIgnore]
        public virtual User User { get; set; }

        public string UserId { get; set; }


    }
}
