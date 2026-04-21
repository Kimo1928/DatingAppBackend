using DatingAppWebApi.Data;
using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Extensions;
using DatingAppWebApi.Helpers;
using DatingAppWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingAppWebApi.Repositories
{
    public class MessageRepository(DatingAppDbContext context) : IMessageRepository
    {
        public void AddGroup(Group group)
        {
            context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
                }

        public async Task<Connection?> GetConnection(string connectionId)
        {
            return await context.Connections.FindAsync(connectionId);
        }

        public async Task<Group?> GetGroupForConnection(string connectionId)
        {
           return await context.Groups
                .Include(x=>x.Connections)
                .Where(x=>x.Connections.Any(c=>c.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public async Task<Message?> GetMessage(string messageId)
        {
          return await  context.Messages.FindAsync(messageId);
        }

        public async Task<Group?> GetMessageGroup(string groupName)
        {
            return await context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<PaginatedResult<MessageDTO>> GetMessagesForUser(MessageParams messagesParams)
        {
            var query = context.Messages.OrderByDescending(x=>x.MessageSent).AsQueryable();
            query = messagesParams.Container switch
            {
                "Outbox" => query.Where(x => x.SenderId == messagesParams.UserId && !x.SenderDeleted),
                _ => query.Where(x => x.RecipientId == messagesParams.UserId && !x.RecipientDeleted )
            };
              var messageQuery = query.Select(MessageExtensions.toDTOProjection());
            return await PaginationHelper.CreateAsync<MessageDTO>(messageQuery, messagesParams.PageNumber, 
                messagesParams.PageSize);
        }

        public async Task<IReadOnlyList<MessageDTO>> GetMessageThread(string currentUserId, string recipientId)
        {
            await context.Messages.Where(m => m.RecipientId == currentUserId && m.SenderId == recipientId 
            && m.DataRead==null).ExecuteUpdateAsync(setters => setters.SetProperty(m => m.DataRead, DateTime.UtcNow));

            return   await context.Messages
                .Where(m => (m.RecipientId == currentUserId && m.SenderId == recipientId && !m.RecipientDeleted ||
                            (m.RecipientId == recipientId && m.SenderId == currentUserId && !m.SenderDeleted )))
                .OrderBy(m => m.MessageSent)
                .Select(MessageExtensions.toDTOProjection())
                .ToListAsync();

        }

        public async Task RemoveConnection(string connectionId)
        {
            await context.Connections.Where(x => x.ConnectionId == connectionId)
                 .ExecuteDeleteAsync();
        }

        
    }
}
