using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MySuperApi.Domain.MongoEntities.User;

namespace MySuperApi.Domain.MongoEntities.Chat;

public class Chat_M
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? LastMessage { get; set; }

    public Guid? ChatAdministrator { get; set; }

    public ICollection<ChatMessage_M> Messages { get; set; } = new List<ChatMessage_M>();
    public ICollection<AppUser_M> ChatUsers { get; set; } = new List<AppUser_M>();
}