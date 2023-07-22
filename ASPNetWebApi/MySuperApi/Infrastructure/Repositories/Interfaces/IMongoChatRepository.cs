using MySuperApi.Api.DTOs;
using MySuperApi.Domain.MongoEntities.Chat;

namespace MySuperApi.Infrastructure.Repositories.Interfaces;

public interface IMongoChatRepository
{
    public Task<Chat_M> CreateChat(CreateChatDto chatDto);
    public Task<List<Chat_M>> GetChatsForUser(string userId);
    public Task<Chat_M> UpdateChat(CreateChatDto chatDto);
    public Task DeleteChat(string chatId);
    public Task<Chat_M> GetChatDetails(string userId, string chatId);
}