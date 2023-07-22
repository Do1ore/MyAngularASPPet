using MySuperApi.Domain.MessageModels;

namespace MySuperApi.Infrastructure.Repositories.Interfaces;

public interface IChatMessageRepository
{
    public Task<ChatMessage> SendMessage(string chatId, string senderId, string messageContent);
}