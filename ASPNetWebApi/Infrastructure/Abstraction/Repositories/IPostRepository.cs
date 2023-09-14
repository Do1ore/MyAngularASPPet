using Domain.MongoEntities.UserProfile;

namespace Infrastructure.Abstraction.Repositories;

public interface IPostRepository
{
    Task CreatePost(PostM postM);
    Task<PostM?> DeletePost(Guid postId);
    Task<List<PostM>> GetPostsById(Guid userId);
}