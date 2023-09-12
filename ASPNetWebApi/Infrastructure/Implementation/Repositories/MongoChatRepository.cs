using Domain.Constants;
using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Implementation.Repositories;

public class MongoChatRepository : IMongoChatRepository
{
    private readonly IMongoCollection<ChatM> _chatCollection;

    public MongoChatRepository(IMongoDatabase database)
    {
        _chatCollection = database.GetCollection<ChatM>(MongoCollectionName.Chat);
    }

    public async Task<ChatM> CreateChat(ChatM chat)
    {
        await _chatCollection.InsertOneAsync(chat);
        return chat;
    }

    public Task<ChatM> UpdateChat(ChatM chat)
    {
        throw new NotImplementedException();
    }

    public async Task<ChatM> GetChatDetails(Guid userId, Guid chatId)
    {
        var filter = Builders<ChatM>.Filter.Eq(chat => chat.Id, chatId);
        var chat = await _chatCollection.Find(filter).FirstOrDefaultAsync();

        if (chat is null)
        {
            throw new ArgumentException("Chat with this id not found");
        }

        if (!chat.AppUserIds.Contains(userId))
        {
            throw new ArgumentException("You don't have access to this chat");
        }

        return chat;
    }

    public async Task<long> DeleteChat(Guid chatId, Guid adminId)
    {
        var filter = Builders<ChatM>.Filter.Eq(chat => chat.Id, chatId);

        var deleteResult = await _chatCollection.DeleteOneAsync(filter);
        return deleteResult.DeletedCount;
    }

    public async Task<bool> IsUserChatAdministrator(Guid chatId, Guid userId)
    {
        var filter = Builders<ChatM>.Filter.Eq(chat => chat.Id, chatId);

        var chat = await _chatCollection.Find(filter).FirstOrDefaultAsync();

        return chat.ChatAdministrator == userId;
    }
}