namespace DatingAppWebApi.Entities
{
    public class UserLike
    {
        public required string SourceUserId { get; set; }

        public User SourceUser { get; set; }

       public required string TargetUserId { get; set; }
        public User TargetUser { get; set; }
    }
}
