using DatingAppWebApi.Data;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Helpers;

namespace DatingAppWebApi.Interfaces
{
    public interface IUserRepository
    {

        public Task<PaginatedResult<User>> GetAllUsersAsync(UserParams userParams);

        public void UpdateUser(User user);

        Task<bool> SaveAllAsync();

        public Task<User?> GetUserByIdAsync(string id);


        public Task<IReadOnlyList<Photo>> GetUserPhotos(string userId);

        public Task<User?> GetUserForUpdate(string userId);


    }
}
