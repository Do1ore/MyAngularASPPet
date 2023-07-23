using Domain.MongoEntities.Chat;

namespace Infrastructure.Abstraction.Repositories;

public interface IMongoChatRepository
{
    public Task<Chat_M> CreateChat(Chat_M chatDto);
    
    public Task<Chat_M> UpdateChat(Chat_M chatDto);
    public Task DeleteChat(string chatId);
    public Task<Chat_M> GetChatDetails(string userId, string chatId);
}