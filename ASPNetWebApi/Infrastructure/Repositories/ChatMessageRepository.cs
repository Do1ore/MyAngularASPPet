using Domain.Constants;
using Domain.MongoEntities.Chat;
using Infrastructure.Abstraction;
using Infrastructure.Abstraction.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly IMongoCollection<ChatM> _chatCollection;

    public ChatMessageRepository(IMongoDatabase database)
    {
        _chatCollection = database.GetCollection<ChatM>(MongoCollectionName.Chat);
    }

    public async Task<ChatMessageM> AddMessage(ChatMessageM message)
    {
        message.Id = Guid.NewGuid();
        message.SentAt = DateTime.UtcNow;

        var filter = Builders<ChatM>.Filter.Eq(chat => chat.Id, message.ChatId);

        var chat = await _chatCollection.Find(filter).ToListAsync();
        var update = Builders<ChatM>.Update.AddToSet<ChatMessageM>(chat => chat.Messages, message);
        var options = new FindOneAndUpdateOptions<ChatM>
        {
            ReturnDocument = ReturnDocument.After
        };
        var result = await _chatCollection.FindOneAndUpdateAsync(filter, update, options);

        return result.Messages.LastOrDefault()!;
    }
}