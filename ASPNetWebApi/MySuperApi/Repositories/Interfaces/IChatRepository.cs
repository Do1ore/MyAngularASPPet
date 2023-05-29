using MySuperApi.Models.MessageModels;

namespace MySuperApi.Repositories.Interfaces
{
    public interface IChatRepository
    {

        public Task<string> GetProfileImage(string userId);
        public Task<ChatMessage> SendMessage(string chatId, string senderId, string messageContent);
        public Task<List<Chat>> GetChatsForUser(string userId);
        public Task<Chat> GetChatDetails(string userId, string chatId);


        Task UpdateCurrentProfileImage(string imageId, string userId);


    }
}
