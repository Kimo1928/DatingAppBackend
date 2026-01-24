using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Helpers;

namespace DatingAppWebApi.Interfaces
{
    public interface IMessageRepository
    {
        void  AddMessage(Message message);
        void  DeleteMessage(Message message);

        Task<Message?> GetMessage(string messageId);

        Task<PaginatedResult<MessageDTO>> GetMessagesForUser(MessageParams messagesParams);

        Task<IReadOnlyList<MessageDTO>> GetMessageThread(string currentUserId, string recipientId);

        Task<bool> SaveAllChanges();
    }
}
