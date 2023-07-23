using Domain.MongoEntities.Chat;

namespace Infrastructure.Abstraction.Repositories;

public interface IMongoChatRepository
{
    public Task<ChatM> CreateChat(ChatM chatDto);
    
    public Task<ChatM> UpdateChat(ChatM chatDto);
    public Task DeleteChat(string chatId);
    public Task<ChatM> GetChatDetails(string userId, string chatId);
}