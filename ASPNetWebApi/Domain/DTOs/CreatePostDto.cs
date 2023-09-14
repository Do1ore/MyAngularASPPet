using Domain.MongoEntities.UserProfile;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs;

public class CreatePostDto
{
    public Guid Id { get; set; }

    public Guid CreatorId { get; set; }

    public string Content { get; set; } = string.Empty;

    public IFormFile? PostImageFile { get; set; }


    public static implicit operator CreatePostDto(PostM postM)
    {
        return new CreatePostDto
        {
            Id = postM.Id,
            CreatorId = postM.CreatorId,
            Content = postM.Content,
        };
    }

    public static implicit operator PostM(CreatePostDto postM)
    {
        return new PostM()
        {
            Id = postM.Id,
            CreatorId = postM.CreatorId,
            Content = postM.Content,
        };
    }
}