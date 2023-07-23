using Infrastructure.Abstraction.Services.User;
using Microsoft.AspNetCore.SignalR;

namespace MySuperApi.HubConfig
{
    public class UserHub : Hub
    {
        private readonly ILogger<UserHub> _logger;

        private readonly IHttpUserDataAccessorService _httpUserDataAccessorService;
        private readonly IHubContext<UserHub> _hubContext;

        public UserHub(ILogger<UserHub> logger,

            IHttpUserDataAccessorService httpUserDataAccessorService,
            IHubContext<UserHub> hubContext)
        {
            _hubContext = hubContext;
            _logger = logger;
            _httpUserDataAccessorService = httpUserDataAccessorService;
        }
    }
}

//         public async Task GetAllChatsForUser(string userId)
//         {
//             if (userId == null)
//             {
//                 return;
//             }
//
//             var chats = await _legacyChatRepository.GetChatsForUser(userId);
//
//             await Clients.Caller.SendAsync("GetAllChatsForUserResponse", chats);
//         }
//
//         public async Task GetChatsDetails(string userId, string chatId)
//         {
//             if (chatId == null)
//             {
//                 return;
//             }
//
//             var chat = await _legacyChatRepository.GetChatDetails(userId, chatId);
//
//             await Clients.Caller.SendAsync("GetChatsDetailsResponse", chat);
//         }
//
//         public async Task SendMessage(string chatId, string senderId, string message)
//         {
//             var chatMessage = await _legacyChatRepository.SendMessage(chatId, senderId, message);
//             var detailedChatMessage = await _legacyChatRepository.GetMessageDetails(chatMessage);
//
//             // Обработка полученного сообщения и отправка его обратно клиентам
//             await Clients.Group(chatId).SendAsync("ReceiveMessage", detailedChatMessage);
//             await Clients.Group(chatId).SendAsync("ReceiveLastMessage", chatId, message);
//         }
//
//         public async Task JoinChat(string chatId)
//         {
//             await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
//             _logger.LogInformation($"Connected to chat {chatId}  and connection {Context.ConnectionId}");
//         }
//
//         public async Task LeaveChat(string chatId)
//         {
//             // Отсоединение клиента от группы чата
//             await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
//             _logger.LogInformation($"Disconnected from chat {chatId}  and connection {Context.ConnectionId}");
//         }
//
//         public async Task CreateChat(CreateChatDto chatDto)
//         {
//             var chat = await _legacyChatRepository.CreateChat(chatDto);
//             _logger.LogInformation("Chat created, users in chat: {Count}", chatDto.UserIds.Count().ToString());
//             await Clients.Caller.SendAsync("CreateChatResponse", chat);
//
//             await JoinChat(chatDto.UserIds.Last());
//         }
//
//         public async Task DeleteChat(string chatId)
//         {
//             await _legacyChatRepository.DeleteChat(chatId);
//             await Clients.Groups(chatId).SendAsync("DeleteChatResponse", chatId);
//             await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
//         }
//
//         public override Task OnConnectedAsync()
//         {
//             _logger.LogInformation("New connection: " + Context.ConnectionId);
//             return base.OnConnectedAsync();
//         }
//
//         public override Task OnDisconnectedAsync(Exception? exception)
//         {
//             _logger.LogInformation("Connection " + Context.ConnectionId + " terminated");
//             return base.OnDisconnectedAsync(exception);
//         }
//     }
// }