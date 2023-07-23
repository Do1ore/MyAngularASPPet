
using Domain.MongoEntities.Chat;

namespace Infrastructure.Abstraction.Repositories;

public interface IChatMessageRepository
{
    public Task<ChatMessage_M> AddMessage(ChatMessage_M message);
}