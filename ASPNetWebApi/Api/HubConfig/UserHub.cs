using Application.Features.Chat.ChatDetails;
using Application.Features.Chat.CreateChat;
using Application.Features.Chat.GetAllChats;
using Application.Features.Message.SendMessage;
using Domain.MongoEntities.Chat;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MySuperApi.DTOs;

namespace MySuperApi.HubConfig;

public class UserHub : Hub
{
    private readonly ILogger<UserHub> _logger;
    private readonly IHubContext<UserHub> _hubContext;
    private readonly IMediator _mediator;


    public UserHub(ILogger<UserHub> logger,
        IHubContext<UserHub> hubContext,
        IMediator mediator)
    {
        _hubContext = hubContext;
        _mediator = mediator;
        _logger = logger;
    }


    public async Task GetAllChatsForUser(string userId)
    {
        _logger.LogInformation("Chat all chats for user: [{@UserId}])", userId);
        var chats = await _mediator.Send(new GetAllChatsForUserRequest(Guid.Parse(userId)));

        await Clients.Caller.SendAsync("GetAllChatsForUserResponse");
    }


    public async Task GetChatsDetails(string userId, string chatId)
    {
        _logger.LogInformation("Chat details for chat:[{@ChatId}]; user id: [{@UserId}]", chatId, userId);
        var chat = await _mediator.Send(new ChatDetailsRequest(Guid.Parse(userId), Guid.Parse(chatId)));

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

    public async Task SendMessage(Guid chatId, Guid senderId, string message)
    {
        var sendingResult = await _mediator.Send(new SendMessageRequest(new ChatMessageM
        {
            Content = message,
            ChatId = chatId,
            SenderId = senderId,
            SentAt = DateTime.UtcNow
        }));

        await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", sendingResult);
    }
}


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