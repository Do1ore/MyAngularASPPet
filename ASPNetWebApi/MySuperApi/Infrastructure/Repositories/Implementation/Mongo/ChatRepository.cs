using MySuperApi.Api.DTOs;
using MySuperApi.Domain;
using MySuperApi.Domain.MessageModels;
using MySuperApi.Infrastructure.Repositories.Interfaces;

namespace MySuperApi.Infrastructure.Repositories.Implementation.Mongo;

public class ChatRepository : IChatRepository
{
    public Task<Chat> CreateChat(CreateChatDto chatDto)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetProfileImage(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<ChatMessage> SendMessage(string chatId, string senderId, string messageContent)
    {
        throw new NotImplementedException();
    }

    public Task<List<Chat>> GetChatsForUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<Chat> GetChatDetails(string userId, string chatId)
    {
        throw new NotImplementedException();
    }

    public Task<ChatMessage> GetMessageDetails(ChatMessage message)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCurrentProfileImage(string imageId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<AppUser>> SearchUsers(string searchTerm)
    {
        throw new NotImplementedException();
    }

    public Task DeleteChat(string chatId)
    {
        throw new NotImplementedException();
    }
}