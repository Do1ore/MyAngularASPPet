using Microsoft.AspNetCore.SignalR;
using MySuperApi.Models.MessageModels;
using MySuperApi.Repositories.Interfaces;
using MySuperApi.Services.UserService;

namespace MySuperApi.HubConfig
{
    public class UserHub : Hub
    {
        private readonly ILogger<UserHub> _logger;
        private readonly IChatRepository _chatRepository;
        private readonly IUserService _userService;

        public UserHub(ILogger<UserHub> logger, IChatRepository chatRepository, IUserService userService)
        {

            _logger = logger;
            _chatRepository = chatRepository;
            _userService = userService;
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

        public async Task<List<Chat>> GetAllChatsForUser(Guid userId)
        {
            var chats = await _chatRepository.GetAllChatsForUser(userId.ToString());
            return default!;
        }

        public async Task SendMessage(string chatId, string senderId, string messageContent)
        {
            await _chatRepository.SendMessage(chatId, senderId, messageContent);
        }
    }
}
