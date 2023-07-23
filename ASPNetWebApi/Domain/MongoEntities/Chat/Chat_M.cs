using Domain.MongoEntities.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.MongoEntities.Chat;

public class Chat_M
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? LastMessage { get; set; }

    [BsonRepresentation(BsonType.String)] public Guid? ChatAdministrator { get; set; }

    [BsonIgnore] public List<Guid>? UserIds { get; set; } = new List<Guid>();
    public ICollection<ChatMessage_M> Messages { get; set; } = new List<ChatMessage_M>();
    public ICollection<AppUser_M> ChatUsers { get; set; } = new List<AppUser_M>();
}