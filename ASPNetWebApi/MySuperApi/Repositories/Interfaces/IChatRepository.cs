using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using MySuperApi.DTOs;
using MySuperApi.Models;
using MySuperApi.Models.MessageModels;

namespace MySuperApi.Repositories.Interfaces
{
    public interface IChatRepository
    {
        public Task<Chat> CreateChat(CreateChatDto chatDto);
        public Task<string> GetProfileImage(string userId);
        public Task<ChatMessage> SendMessage(string chatId, string senderId, string messageContent);
        public Task<List<Chat>> GetChatsForUser(string userId);
        public Task<Chat> GetChatDetails(string userId, string chatId);
        public Task<ChatMessage> GetMessageDetails(ChatMessage message);
        Task UpdateCurrentProfileImage(string imageId, string userId);

        public Task<List<AppUser>> SearchUsers(string searchTerm);


    }
}
