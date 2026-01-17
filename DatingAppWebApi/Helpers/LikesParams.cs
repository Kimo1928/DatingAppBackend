namespace DatingAppWebApi.Helpers
{
    public class LikesParams :PagingParams
    {
        public string UserId { get; set; }="";

        public string Predicate { get; set; } = "liked";
    }
}
