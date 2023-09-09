using Application.Features.Chat.ChatDetails;
using Application.Features.Chat.CreateChat;
using Application.Features.Chat.DeleteChat;
using Application.Features.Chat.GetAllChats;
using Application.Features.Message.SendMessage;
using Domain.DTOs;
using Domain.MongoEntities.Chat;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace MySuperApi.HubConfig;

public class UserHub : Hub
{
    private readonly ILogger<UserHub> _logger;
    private readonly IMediator _mediator;


    public UserHub(ILogger<UserHub> logger,
        IMediator mediator)
    {
        _mediator = mediator;
        _logger = logger;
    }


    public async Task GetAllChatsForUser(string userId)
    {
        _logger.LogInformation("Chat all chats for user: [{@UserId}])", userId);
        var chats = await _mediator.Send(new GetAllChatsForUserRequest(Guid.Parse(userId)));

        await Clients.Caller.SendAsync("GetAllChatsForUserResponse", chats);
    }


    public async Task GetChatsDetails(string userId, string chatId)
    {
        await JoinChat(Guid.Parse(chatId));
        _logger.LogInformation("Chat details for chat:[{@ChatId}]; user id: [{@UserId}]", chatId, userId);
        var chat = await _mediator.Send(new ChatDetailsRequest(Guid.Parse(userId), Guid.Parse(chatId)));

        await Clients.Caller.SendAsync("GetChatsDetailsResponse", chat);
    }

    public async Task CreateChat(CreateChatDto chatDto)
    {
        var chat = await _mediator.Send(new CreateChatRequest(chatDto));
        await Clients.Caller.SendAsync("CreateChatResponse", chat);

        await JoinChat(chat.Id);
    }

    private async Task JoinChat(Guid chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        _logger.LogInformation("Connected to chat {ChatId}  and connection {ContextConnectionId}", chatId,
            Context.ConnectionId);
    }

    public async Task SendMessage(string chatId, string senderId, string message)
    {
        var sendingResult = await _mediator.Send(new SendMessageRequest(new ChatMessageM
        {
            Content = message,
            ChatId = Guid.Parse(chatId),
            SenderId = Guid.Parse(senderId),
            SentAt = DateTime.UtcNow
        }));

        await Clients.Group(chatId).SendAsync("ReceiveMessage", sendingResult);
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("New connection: {@Message}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public async Task DeleteChat(string chatId, string userId)
    {
        await _mediator.Send(new DeleteChatRequest(Guid.Parse(chatId), Guid.Parse(userId)));
        await Clients.Groups(chatId).SendAsync("DeleteChatResponse", chatId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
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


//         public override Task OnDisconnectedAsync(Exception? exception)
//         {
//             _logger.LogInformation("Connection " + Context.ConnectionId + " terminated");
//             return base.OnDisconnectedAsync(exception);
//         }
//     }
// }