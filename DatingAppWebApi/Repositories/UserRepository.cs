using DatingAppWebApi.Data;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Helpers;
using DatingAppWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatingAppDbContext _context;
        public UserRepository(DatingAppDbContext context)
        {
            _context = context;
        }
        public async Task<PaginatedResult<User>> GetAllUsersAsync(UserParams userParams)
        {
           var query=  _context.Users.AsQueryable();

            query = query.Where(x => x.Id != userParams.CurrentUserId);

            if (userParams.Gender!=null) {
                query = query.Where(x => x.Gender == userParams.Gender);
            }
            var minDob= DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
            query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(x => x.Created),
                _ => query.OrderBy(x => x.LastActive)
            };
            return await PaginationHelper.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id); ;

        }

       

        public async Task<IReadOnlyList<Photo>> GetUserPhotos(string userId)
        {
            var photos= await _context.Photos.Where(p => p.UserId == userId).ToListAsync();
            return photos;
        }

        public async Task<bool> SaveAllAsync()
        {
           return await _context.SaveChangesAsync()>0;
        }

        public void UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        
        }
        public Task<User?> GetUserForUpdate(string userId)
        {
            return _context.Users.Include(x => x.AppUser)
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
