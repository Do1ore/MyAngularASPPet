using Microsoft.AspNetCore.Http.HttpResults;
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
           
            if(userId == null)
            {
                return;
            }
            var chats = await _chatRepository.GetAllChatsForUser(userId);

            await Clients.Caller.SendAsync("GetAllChatsForUserResponse", chats);
        }

        public async Task SendMessage(string chatId, string senderId, string messageContent)
        {
            await _chatRepository.SendMessage(chatId, senderId, messageContent);
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
