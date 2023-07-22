using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MySuperApi.Domain.MongoEntities.Chat;

public sealed class ChatMessage_M
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public DateTime SentAt { get; set; }


    public Guid SenderId { get; set; }
    public AppUser? Sender { get; set; }

    public Guid ChatId { get; set; }
    [BsonIgnore]
    public MessageModels.Chat? Chat { get; set; }
}