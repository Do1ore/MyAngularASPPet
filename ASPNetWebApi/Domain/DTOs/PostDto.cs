using Domain.MongoEntities.UserProfile;

namespace Domain.DTOs;

public class PostDto
{
    public Guid Id { get; set; }
    
    public Guid CreatorId { get; set; }
    
    public string Content { get; set; } = string.Empty;
    
    public string? PostImagePath { get; set; } = string.Empty;
    
    public DateTime Created { get; set; }


    public static implicit operator PostDto(PostM postM)
    {
        return new PostDto
        {
            Id = postM.Id,
            CreatorId = postM.CreatorId,
            Content = postM.Content,
            PostImagePath = postM.PostImagePath,
            Created = postM.Created,
        };
    }
}