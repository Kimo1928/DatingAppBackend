using DatingAppWebApi.Entities;

namespace DatingAppWebApi.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLike(string sourceUserId, string likedUserId);

        Task<IReadOnlyList<User>> GetUserLikes(string predicate , string userId);


        Task<IReadOnlyList<string>> GetCurrentUserLikeIds(string userId);


        void AddLike(UserLike userLike);

        void RemoveLike(UserLike userLike);


        Task<bool> SaveAllChanges();
    }
}
