using Domain.Constants;
using Domain.MongoEntities.UserProfile;
using Infrastructure.Abstraction.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Implementation.Repositories;

public class PostRepository : IPostRepository
{
    private readonly IMongoCollection<PostM> _postCollection;

    public PostRepository(IMongoDatabase database)
    {
        _postCollection = database.GetCollection<PostM>(MongoCollectionName.Post);
    }

    public async Task CreatePost(PostM postM)
    {
        await _postCollection.InsertOneAsync(postM);
    }

    public async Task<PostM?> DeletePost(Guid postId)
    {
        var filter = Builders<PostM>.Filter.Eq(a => a.Id, postId);
        var result =  await _postCollection.FindOneAndDeleteAsync(filter);
        return result;
    }

    public async Task<List<PostM>> GetPostsById(Guid userId)
    {
        var filter = Builders<PostM>.Filter.Eq(a => a.CreatorId, userId);
        var results = await _postCollection.Find(filter).ToListAsync();
        return results;
    }
}