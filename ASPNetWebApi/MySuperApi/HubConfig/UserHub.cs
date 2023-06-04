using Azure.Messaging;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using MySuperApi.DTOs;
using MySuperApi.Migrations;
using MySuperApi.Models.MessageModels;
using MySuperApi.Repositories.Interfaces;
using MySuperApi.Services.UserService;
using NuGet.Protocol.Plugins;

namespace MySuperApi.HubConfig
{
    public class UserHub : Hub
    {
        private readonly ILogger<UserHub> _logger;
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;
        private readonly IHubContext<UserHub> _hubContext;

        public UserHub(ILogger<UserHub> logger,
                       IChatRepository chatRepository,
                       IUserService userService,
                       IHubContext<UserHub> hubContext)
        {

            _hubContext = hubContext;
            _logger = logger;
            _chatRepository = chatRepository;
            _userService = userService;
        }

        //public async Task getOnlineUsers()
        //{
        //    Guid currUserId = ctx.Connections.Where(c => c.SignalrId == Context.ConnectionId).Select(c => c.PersonId).SingleOrDefault();
        //    List<User> onlineUsers = ctx.Connections
        //        .Where(c => c.PersonId != currUserId)
        //        .Select(c =>
        //            new User(c.PersonId, ctx.Person.Where(p => p.Id == c.PersonId).Select(p => p.Name).SingleOrDefault(), c.SignalrId)
        //        ).ToList();
        //    await Clients.Caller.SendAsync("getOnlineUsersResponse", onlineUsers);
        //}


        //public async Task sendMsg(string connId, string msg)
        //{
        //    await Clients.Client(connId).SendAsync("sendMsgResponse", Context.ConnectionId, msg);
        //}

        public async Task GetAllChatsForUser(string userId)
        {

            if (userId == null)
            {
                return;
            }
            var chats = await _chatRepository.GetChatsForUser(userId);

            await Clients.Caller.SendAsync("GetAllChatsForUserResponse", chats);
        }

        public async Task GetChatsDetails(string userId, string chatId)
        {
            if (chatId == null)
            {
                return;
            }
            var chat = await _chatRepository.GetChatDetails(userId, chatId);

            await Clients.Caller.SendAsync("GetChatsDetailsResponse", chat);
        }
        public async Task SendMessage(string chatId, string senderId, string message)
        {
            var chatMessage = await _chatRepository.SendMessage(chatId, senderId, message);
            var detailedChatMessage = await _chatRepository.GetMessageDetails(chatMessage);

            // Обработка полученного сообщения и отправка его обратно клиентам
            await Clients.Group(chatId).SendAsync("ReceiveMessage", detailedChatMessage);
            await Clients.Group(chatId).SendAsync("ReceiveLastMessage", chatId, message);
        }
        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
            _logger.LogInformation($"Connected to chat {chatId}  and connection {Context.ConnectionId}");
        }
        public async Task LeaveChat(string chatId)
        {
            // Отсоединение клиента от группы чата
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
            _logger.LogInformation($"Disconnected from chat {chatId}  and connection {Context.ConnectionId}");
        }

        public async Task CreateChat(CreateChatDto chatDto)
        {
            var chat = await _chatRepository.CreateChat(chatDto);
            _logger.LogInformation($"Chat created, users in chat: ", chatDto!.UserId!.Count);
            await Clients.Caller.SendAsync("CreateChatResponse", chat);

            await JoinChat(chatDto.UserId.Last());
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("New connection: " + Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Connection " + Context.ConnectionId + " terminated");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
