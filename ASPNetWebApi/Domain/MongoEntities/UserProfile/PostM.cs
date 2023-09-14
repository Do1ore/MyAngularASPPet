namespace Domain.MongoEntities.UserProfile;

public class PostM
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid CreatorId { get; set; }
    
    public string Content { get; set; } = string.Empty;
    
    public string? PostImagePath { get; set; } = string.Empty;
    
    public DateTime Created { get; set; }
    
}