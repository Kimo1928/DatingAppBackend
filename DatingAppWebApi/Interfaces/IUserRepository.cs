using DatingAppWebApi.Data;
using DatingAppWebApi.Entities;

namespace DatingAppWebApi.Interfaces
{
    public interface IUserRepository
    {

        public Task<IReadOnlyList<User>> GetAllUsersAsync();

        public void UpdateUser(User user);

        Task<bool> SaveAllAsync();

        public Task<User?> GetUserByIdAsync(string id);


        public Task<IReadOnlyList<Photo>> GetUserPhotos(string userId);

        public Task<User?> GetUserForUpdate(string userId);


    }
}
