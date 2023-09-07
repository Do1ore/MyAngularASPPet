
using Domain.MongoEntities.Chat;

namespace Infrastructure.Abstraction.Repositories;

public interface IMongoChatMessageRepository
{
    public Task<ChatMessageM> AddMessage(ChatMessageM message);
}