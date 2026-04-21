using DatingAppWebApi.Data;
using DatingAppWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.Repositories
{
    public class UnitOfWork(DatingAppDbContext context) : IUnitOfWork
    {
        private IMessageRepository? _messageRepository;
        private IUserRepository? _userRepository;
        private ILikesRepository? _likesRepository;
        public IMessageRepository MessageRepository => _messageRepository?? new MessageRepository(context);

        public ILikesRepository LikesRepository => _likesRepository ?? new LikesRepository(context);

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(context);

        public async Task<bool> Complete()
        {
            try
            {
                return await context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException ex) {
                throw new Exception("An error occured while saving changes",ex );
            }
        }

        public bool HasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }
    }
}
