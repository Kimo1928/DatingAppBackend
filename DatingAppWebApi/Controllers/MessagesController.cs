using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Extensions;
using DatingAppWebApi.Helpers;
using DatingAppWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppWebApi.Controllers
{
    public class MessagesController(IUnitOfWork unitOfWork) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDTO createMessageDTO)
        {
            var sender = await unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());
            var recipient = await unitOfWork.UserRepository.GetUserByIdAsync(createMessageDTO.RecipientId);
            if (recipient == null || sender == null || sender.Id == recipient.Id) return NotFound("Can't send this Message");

            var message = new Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Content = createMessageDTO.Content,
            };
            unitOfWork.MessageRepository.AddMessage(message);
            if (await unitOfWork.Complete())
            {
                return Ok(message.toDto());
            }
            return BadRequest("Failed to send Message");

        }
        [HttpGet]
        public async Task<IActionResult> GetMessagesByContainer([FromQuery] MessageParams messageParams)
        {

            messageParams.UserId = User.GetUserId();
            return Ok(await unitOfWork.MessageRepository.GetMessagesForUser(messageParams));

        }
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(string recipientId)
        {
            var currentUserId = User.GetUserId();
            var messages = await unitOfWork.MessageRepository.GetMessageThread(currentUserId, recipientId);
            return Ok(messages);
        }
    
             [HttpDelete("{messageId}")]
         public async Task<IActionResult> DeleteMessage(string messageId) {
            var userId = User.GetUserId();
            var message = await unitOfWork.MessageRepository.GetMessage(messageId);
            if (message == null) return BadRequest("Cannot delete this message");

            if (message.SenderId != userId && message.RecipientId != userId) return BadRequest("You cannot delete this message");

            if (message.SenderId == userId) message.SenderDeleted = true;
            if (message.RecipientId == userId) message.RecipientDeleted = true;
            if (message.SenderDeleted && message.RecipientDeleted)
            {
                unitOfWork.MessageRepository.DeleteMessage(message);
            }
            if (await unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to delete the message");


        }
    } }
