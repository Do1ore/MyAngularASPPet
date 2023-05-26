using MySuperApi.Models.MessageModels;

namespace MySuperApi.Repositories.Interfaces
{
    public interface IChatRepository
    {
        public Task<List<Chat>>GetAllChatsForUser(string userId);

        public Task SendMessage(string chatId, string senderId, string messageContent);

    }
}
