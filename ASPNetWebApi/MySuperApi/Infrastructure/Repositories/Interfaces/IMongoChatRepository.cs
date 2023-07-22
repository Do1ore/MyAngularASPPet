using MySuperApi.Api.DTOs;
using MySuperApi.Domain.MessageModels;

namespace MySuperApi.Infrastructure.Repositories.Interfaces;

public interface IMongoChatRepository
{
    public Task<Chat> CreateChat(CreateChatDto chatDto);
    public Task<List<Chat>> GetChatsForUser(string userId);
    public Task<Chat> UpdateChat(CreateChatDto chatDto);
    public Task DeleteChat(string chatId);
    public Task<Chat> GetChatDetails(string userId, string chatId);
}