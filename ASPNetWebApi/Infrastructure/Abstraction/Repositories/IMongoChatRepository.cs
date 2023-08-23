using Domain.MongoEntities.Chat;

namespace Infrastructure.Abstraction.Repositories;

public interface IMongoChatRepository
{
    public Task<ChatM> CreateChat(ChatM chat);
    
    public Task<ChatM> UpdateChat(ChatM chat);
    public Task DeleteChat(Guid chatId);
    public Task<ChatM> GetChatDetails(Guid userId, Guid chatId);
}