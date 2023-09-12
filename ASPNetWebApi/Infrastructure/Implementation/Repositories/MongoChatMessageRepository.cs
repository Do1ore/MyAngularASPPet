using Domain.Constants;
using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Implementation.Repositories;

public class MongoChatMessageRepository : IMongoChatMessageRepository
{
    private readonly IMongoCollection<ChatM> _chatCollection;

    public MongoChatMessageRepository(IMongoDatabase database)
    {
        _chatCollection = database.GetCollection<ChatM>(MongoCollectionName.Chat);
    }

    public async Task<ChatMessageM> AddMessage(ChatMessageM message)
    {
        message.Id = Guid.NewGuid();
        message.SentAt = DateTime.UtcNow;

        var filter = Builders<ChatM>.Filter.Eq(chat => chat.Id, message.ChatId);
        var update = Builders<ChatM>.Update.AddToSet(chat => chat.Messages, message);
        var updateLastMessage = Builders<ChatM>.Update.Set(chat => chat.LastMessage, message.Content);
        
        //add message to chat and update last message
        var combinedUpdate = Builders<ChatM>.Update.Combine(update, updateLastMessage);

        var options = new FindOneAndUpdateOptions<ChatM>
        {
            ReturnDocument = ReturnDocument.After
        };
        
        var result = await _chatCollection.FindOneAndUpdateAsync(filter, combinedUpdate, options);

        return result.Messages.LastOrDefault()!;
    }
}