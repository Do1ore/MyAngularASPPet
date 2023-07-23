
using Domain.MongoEntities.Chat;

namespace Infrastructure.Abstraction.Repositories;

public interface IChatMessageRepository
{
    public Task<ChatMessageM> AddMessage(ChatMessageM message);
}