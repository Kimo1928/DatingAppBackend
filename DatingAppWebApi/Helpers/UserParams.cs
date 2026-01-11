namespace DatingAppWebApi.Helpers
{
    public class UserParams : PagingParams
    {
        public string? Gender { get; set; }

        public string? CurrentUserId { get; set; }


        public int MinAge { get; set; } = 18;

        public int MaxAge { get; set; } = 100;

        public string OrderBy { get; set; } = "lastActive";
    }
}
