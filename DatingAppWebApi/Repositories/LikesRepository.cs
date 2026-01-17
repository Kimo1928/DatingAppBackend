using DatingAppWebApi.Data;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Helpers;
using DatingAppWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.Repositories
{
    public class LikesRepository : ILikesRepository
    {

        private readonly DatingAppDbContext _context;
        public LikesRepository(DatingAppDbContext context)
        {
            _context = context;
        }
        public void AddLike(UserLike userLike)
        {
        _context.Likes.Add(userLike);
        }

        public async Task<IReadOnlyList<string>> GetCurrentUserLikeIds(string userId)
        {
            return await _context.Likes
                .Where(x => x.SourceUserId == userId)
                .Select(x => x.TargetUserId)
                .ToListAsync();
        }

        public async  Task<UserLike?> GetUserLike(string sourceUserId, string likedUserId)
        {
            return await _context.Likes.FindAsync
                (sourceUserId,likedUserId);
        }

        public async Task<PaginatedResult<User>> GetUserLikes(LikesParams likesParams)
        {
            var query = _context.Likes.AsQueryable();
            IQueryable<User> result;
            switch (likesParams.Predicate) {

                case "liked":
                    result =  query.Where(x => x.SourceUserId == likesParams.UserId).Select(x => x.TargetUser);
                    break;
                case "likedBy":
                    result= query.Where(x => x.TargetUserId == likesParams.UserId).Select(x => x.SourceUser);
                    break;

                default:
                    var likeIds = await GetCurrentUserLikeIds(likesParams.UserId);
                    result= query.Where(x => likeIds.Contains(x.SourceUserId) && x.TargetUserId==likesParams.UserId).Select(x => x.SourceUser);
                    break;
            }
            return await PaginationHelper.CreateAsync(result, likesParams.PageNumber, likesParams.PageSize);

        }

        public void RemoveLike(UserLike userLike)
        {
            _context.Likes.Remove(userLike);
        }

        public async Task<bool> SaveAllChanges()
        {
         return  await _context.SaveChangesAsync()>0;      
        
        }
    }
}
