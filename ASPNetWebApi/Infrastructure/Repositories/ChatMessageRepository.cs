using Domain.Constants;
using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction;
using Infrastructure.Abstraction.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly IMongoCollection<Chat_M> _chatCollection;

    public ChatMessageRepository(IMongoDatabase database)
    {
        _chatCollection = database.GetCollection<Chat_M>(MongoCollectionName.Chat);
    }

    public async Task<ChatMessage_M> AddMessage(ChatMessage_M message)
    {
        message.Id = Guid.NewGuid();
        message.SentAt = DateTime.UtcNow;

        var filter = Builders<Chat_M>.Filter.Eq(chat => chat.Id, message.ChatId);

        var chat = await _chatCollection.Find(filter).ToListAsync();
        var update = Builders<Chat_M>.Update.AddToSet<ChatMessage_M>(chat => chat.Messages, message);
        var options = new FindOneAndUpdateOptions<Chat_M>
        {
            ReturnDocument = ReturnDocument.After
        };
        var result = await _chatCollection.FindOneAndUpdateAsync(filter, update, options);

        return result.Messages.LastOrDefault()!;
    }
}