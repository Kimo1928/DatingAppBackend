using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using System.Linq.Expressions;

namespace DatingAppWebApi.Extensions
{
    public static class MessageExtensions
    {
        public static MessageDTO toDto(this Message message) { 
        
            return new MessageDTO
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderDisplayName = message.Sender.DisplayName,
                SenderImageUrl = message.Sender.ImageUrl,
                RecipientImageUrl = message.Recipient.ImageUrl,
                RecipientId = message.RecipientId,
                RecipientDisplayName = message.Recipient.DisplayName,
                Content = message.Content,
                DataRead = message.DataRead,
                MessageSent = message.MessageSent,
              
            };

        }


        public static Expression<Func<Message,MessageDTO>> toDTOProjection()
        {
            return message => new MessageDTO
            {
                Id = message.Id,
                SenderId = message.SenderId,
                SenderDisplayName = message.Sender.DisplayName,
                SenderImageUrl = message.Sender.ImageUrl,
                RecipientImageUrl = message.Recipient.ImageUrl,
                RecipientId = message.RecipientId,
                RecipientDisplayName = message.Recipient.DisplayName,
                Content = message.Content,
                DataRead = message.DataRead,
                MessageSent = message.MessageSent,
            };
        }
    }
}
