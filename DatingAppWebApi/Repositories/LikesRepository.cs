using DatingAppWebApi.Data;
using DatingAppWebApi.Entities;
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

        public async Task<IReadOnlyList<User>> GetUserLikes(string predicate, string userId)
        {
            var query = _context.Likes.AsQueryable();
            switch (predicate) {

                case "liked":
                    return await query.Where(x => x.SourceUserId == userId).Select(x => x.TargetUser).ToListAsync();
                case "likedBy":
                    return await query.Where(x => x.TargetUserId == userId).Select(x => x.SourceUser).ToListAsync();

                 default:
                    var likeIds = await GetCurrentUserLikeIds(userId);
                    return await query.Where(x => likeIds.Contains(x.SourceUserId) && x.TargetUserId==userId).Select(x => x.SourceUser).ToListAsync();
            }

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
