using Application.Features.Chat.ChatDetails;
using Application.Features.Chat.CreateChat;
using Infrastructure.Abstraction.Services.User;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MySuperApi.DTOs;

namespace MySuperApi.HubConfig;

public class UserHub : Hub
{
    private readonly ILogger<UserHub> _logger;
    private readonly IHttpUserDataAccessorService _httpUserDataAccessorService;
    private readonly IHubContext<UserHub> _hubContext;
    private readonly IMediator _mediator;


    public UserHub(ILogger<UserHub> logger,
        IHttpUserDataAccessorService httpUserDataAccessorService,
        IHubContext<UserHub> hubContext,
        IMediator mediator)
    {
        _hubContext = hubContext;
        _mediator = mediator;
        _logger = logger;
        _httpUserDataAccessorService = httpUserDataAccessorService;
    }


    public async Task GetAllChatsForUser(string? userId)
    {
        if (userId == null)
        {
            return;
        }

        //var chats = await _legacyChatRepository.GetChatsForUser(userId);

        await Clients.Caller.SendAsync("GetAllChatsForUserResponse");
    }


    public async Task GetChatsDetails(Guid userId, Guid chatId)
    {
        var chat = await _mediator.Send(new ChatDetailsRequest(userId, chatId));

        await Clients.Caller.SendAsync("GetChatsDetailsResponse", chat);
    }

    public async Task CreateChat(CreateChatDto chatDto)
    {
        var chat = await _mediator.Send(new CreateChatRequest(chatDto));
        await Clients.Caller.SendAsync("CreateChatResponse", chat);

        await JoinChat((Guid)chat.ChatAdministrator!);
    }

    private async Task JoinChat(Guid chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        _logger.LogInformation("Connected to chat {ChatId}  and connection {ContextConnectionId}", chatId,
            Context.ConnectionId);
    }
}


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

//
//         public async Task LeaveChat(string chatId)
//         {
//             // Отсоединение клиента от группы чата
//             await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
//             _logger.LogInformation($"Disconnected from chat {chatId}  and connection {Context.ConnectionId}");
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