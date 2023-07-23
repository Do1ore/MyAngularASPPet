using Domain.MongoEntities.Chat;
using MediatR;

namespace Application.Features.Chat.CreateChat;

public record CreateChatRequest(ChatM Chat) : IRequest<ChatM>;