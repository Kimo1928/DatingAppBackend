using DatingAppWebApi.Data;
using DatingAppWebApi.Entities;
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
        public async Task<IReadOnlyList<User>> GetAllUsersAsync()
        {
           var users= await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id); ;

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
    }
}
