using Domain.MongoEntities.Chat;
using MediatR;

namespace Application.Features.Chat.CreateChat;

public record CreateChatRequest(Chat_M Chat) : IRequest<Chat_M>;