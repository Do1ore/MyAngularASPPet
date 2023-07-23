using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction;
using Infrastructure.Abstraction.Repositories;

namespace Infrastructure.Repositories;

public class MongoChatRepository : IMongoChatRepository
{
  
    public Task<ChatM> CreateChat(ChatM chatDto)
    {
        throw new NotImplementedException();
    }

    public Task<ChatM> UpdateChat(ChatM chatDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteChat(string chatId)
    {
        throw new NotImplementedException();
    }

    public Task<ChatM> GetChatDetails(string userId, string chatId)
    {
        throw new NotImplementedException();
    }
}
    

   