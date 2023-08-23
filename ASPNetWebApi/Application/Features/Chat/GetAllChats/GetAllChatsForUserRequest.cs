using Domain.MongoEntities.Chat;
using MediatR;

namespace Application.Features.Chat.GetAllChats;

public record GetAllChatsForUserRequest(Guid UserId) : IRequest<List<ChatM>>;