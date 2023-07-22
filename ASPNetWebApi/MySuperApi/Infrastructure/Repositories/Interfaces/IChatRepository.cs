using MySuperApi.Api.DTOs;
using MySuperApi.Domain;
using MySuperApi.Domain.MessageModels;

namespace MySuperApi.Infrastructure.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> CreateChat(CreateChatDto chatDto);
        Task<string> GetProfileImage(string userId);
        Task<ChatMessage> SendMessage(string chatId, string senderId, string messageContent);
        Task<List<Chat>> GetChatsForUser(string userId);
        Task<Chat> GetChatDetails(string userId, string chatId);
        Task<ChatMessage> GetMessageDetails(ChatMessage message);
        Task UpdateCurrentProfileImage(string imageId, string userId);
        Task<List<AppUser>> SearchUsers(string searchTerm);
        Task DeleteChat(string chatId);
    }
}