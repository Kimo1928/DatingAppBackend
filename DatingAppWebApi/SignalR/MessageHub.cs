using DatingAppWebApi.DTOs;
using DatingAppWebApi.Entities;
using DatingAppWebApi.Extensions;
using DatingAppWebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using System.Data;

namespace DatingAppWebApi.SignalR
{
    [Authorize]
    public class MessageHub(IUnitOfWork unitOfWork
        , IHubContext<PresenceHub> presenceHub) : Hub
    {

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request?.Query["userId"].ToString() ??
                throw new HubException("Other user not found ");
            var groupName = GetGroupName(GetUserId(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await AddToGroup(groupName);
            var messages = await unitOfWork.MessageRepository.GetMessageThread(GetUserId(), otherUser);

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);


        }

        public async Task SendMessage(CreateMessageDTO createMessageDTO) {

            var sender = await unitOfWork.UserRepository.GetUserByIdAsync(GetUserId());
            var recipient = await unitOfWork.UserRepository.GetUserByIdAsync(createMessageDTO.RecipientId);
            if (recipient == null || sender == null || sender.Id == recipient.Id)
                throw new HubException("Caannot send message");

            var message = new Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                Content = createMessageDTO.Content,
            };

            var groupName=GetGroupName(sender.Id, recipient.Id);
            var group = await unitOfWork.MessageRepository.GetMessageGroup(groupName);
            var userInGroup = group != null && group.Connections.Any(x => x.UserId == message.RecipientId);
            if (userInGroup) {
                message.DataRead = DateTime.UtcNow;
            
            }
            unitOfWork.MessageRepository.AddMessage(message);
            if (await unitOfWork.Complete())
            {
               
                await Clients.Group(groupName).SendAsync("NewMessage",message.toDto());
                var connections = await PresenceTracker.GetConnectionForUser(recipient.Id);
                if (connections != null && connections.Count > 0 && !userInGroup)
                    await presenceHub.Clients.
                        Clients(connections).SendAsync("NewMessageReceived", message.toDto());

            }
          


        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await unitOfWork.MessageRepository.RemoveConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }


        private async Task<bool> AddToGroup(string groupName) {

            var group = await unitOfWork.MessageRepository.GetMessageGroup(groupName);
            var connection =  new Connection(Context.ConnectionId, GetUserId());
            if (group == null) {
                group = new Group(groupName);
                unitOfWork.MessageRepository.AddGroup(group);
            }
            group.Connections.Add(connection);

          return await unitOfWork.Complete();
        }

        private static string GetGroupName(string? caller, string? other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private string GetUserId() => Context.User?.GetUserId() ?? throw new HubException("Cannot get user Id");
    }
}
