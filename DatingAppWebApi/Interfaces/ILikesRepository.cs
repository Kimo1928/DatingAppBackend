using DatingAppWebApi.Entities;
using DatingAppWebApi.Helpers;

namespace DatingAppWebApi.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLike(string sourceUserId, string likedUserId);

        Task<PaginatedResult<User>> GetUserLikes(LikesParams likesParams);


        Task<IReadOnlyList<string>> GetCurrentUserLikeIds(string userId);


        void AddLike(UserLike userLike);

        void RemoveLike(UserLike userLike);


        Task<bool> SaveAllChanges();
    }
}
