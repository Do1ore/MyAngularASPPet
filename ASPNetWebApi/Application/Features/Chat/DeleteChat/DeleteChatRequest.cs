using MediatR;

namespace Application.Features.Chat.DeleteChat;

public record DeleteChatRequest(Guid ChatId, Guid UserId) : IRequest<long>;