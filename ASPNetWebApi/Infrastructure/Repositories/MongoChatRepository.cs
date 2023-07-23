using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction;
using Infrastructure.Abstraction.Repositories;

namespace Infrastructure.Repositories;

public class MongoChatRepository : IMongoChatRepository
{
  
    public Task<Chat_M> CreateChat(Chat_M chatDto)
    {
        throw new NotImplementedException();
    }

    public Task<Chat_M> UpdateChat(Chat_M chatDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteChat(string chatId)
    {
        throw new NotImplementedException();
    }

    public Task<Chat_M> GetChatDetails(string userId, string chatId)
    {
        throw new NotImplementedException();
    }
}
    

   